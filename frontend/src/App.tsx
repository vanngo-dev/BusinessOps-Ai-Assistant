import { useEffect, useMemo, useState } from "react";
import type { ComponentType } from "react";
import {
  Activity,
  AlertTriangle,
  ArrowUpRight,
  Boxes,
  CircleCheck,
  Clock3,
  DollarSign,
  Headphones,
  PackageSearch,
  RefreshCw,
  Truck
} from "lucide-react";
import { getHealthStatus, getOperationsSummary } from "./api/businessOpsApi";
import type { HealthStatus, OperationsSummary } from "./types/operations";
import "./App.css";

type MetricTone = "blue" | "amber" | "red" | "green" | "slate";

type Metric = {
  label: string;
  value: string;
  detail: string;
  tone: MetricTone;
  icon: ComponentType<{ size?: number; strokeWidth?: number }>;
};

const currencyFormatter = new Intl.NumberFormat("en-US", {
  style: "currency",
  currency: "USD",
  maximumFractionDigits: 0
});

function formatCurrency(value: number) {
  return currencyFormatter.format(value);
}

function App() {
  const [summary, setSummary] = useState<OperationsSummary | null>(null);
  const [health, setHealth] = useState<HealthStatus | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  async function loadDashboard() {
    try {
      setIsLoading(true);
      setError(null);

      const [summaryResult, healthResult] = await Promise.all([
        getOperationsSummary(),
        getHealthStatus()
      ]);

      setSummary(summaryResult);
      setHealth(healthResult);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Unable to load dashboard data");
    } finally {
      setIsLoading(false);
    }
  }

  useEffect(() => {
    void loadDashboard();
  }, []);

  const metrics = useMemo<Metric[]>(() => {
    if (!summary) {
      return [];
    }

    return [
      {
        label: "Open order value",
        value: formatCurrency(summary.estimatedOpenOrderValue),
        detail: `${summary.openOrders} orders still active`,
        tone: "blue",
        icon: DollarSign
      },
      {
        label: "Low-stock products",
        value: summary.lowStockProducts.toString(),
        detail: `${summary.totalProducts} products monitored`,
        tone: summary.lowStockProducts > 0 ? "amber" : "green",
        icon: Boxes
      },
      {
        label: "Delayed shipments",
        value: summary.delayedShipments.toString(),
        detail: "Needs fulfillment review",
        tone: summary.delayedShipments > 0 ? "red" : "green",
        icon: Truck
      },
      {
        label: "Missing tracking",
        value: summary.ordersMissingTracking.toString(),
        detail: "Shipment records incomplete",
        tone: summary.ordersMissingTracking > 0 ? "amber" : "green",
        icon: PackageSearch
      },
      {
        label: "High-priority issues",
        value: summary.highPrioritySupportIssues.toString(),
        detail: "Customer-facing pressure",
        tone: summary.highPrioritySupportIssues > 0 ? "red" : "green",
        icon: AlertTriangle
      },
      {
        label: "Unresolved support",
        value: summary.unresolvedSupportIssues.toString(),
        detail: "Tickets still open",
        tone: summary.unresolvedSupportIssues > 0 ? "slate" : "green",
        icon: Headphones
      }
    ];
  }, [summary]);

  const riskLevel = useMemo(() => {
    if (!summary) {
      return "Loading";
    }

    const riskScore =
      summary.lowStockProducts +
      summary.delayedShipments * 2 +
      summary.ordersMissingTracking +
      summary.highPrioritySupportIssues * 2;

    if (riskScore >= 12) {
      return "Elevated";
    }

    if (riskScore >= 6) {
      return "Moderate";
    }

    return "Stable";
  }, [summary]);

  return (
    <main className="app-shell">
      <aside className="sidebar" aria-label="Main navigation">
        <div className="brand-lockup">
          <span className="brand-mark">
            <Activity size={20} strokeWidth={2.4} />
          </span>
          <div>
            <p className="brand-name">BusinessOps</p>
            <p className="brand-subtitle">AI Assistant</p>
          </div>
        </div>

        <nav className="nav-list">
          <a className="nav-item active" href="#overview">
            <CircleCheck size={17} />
            Overview
          </a>
          <a className="nav-item" href="#inventory">
            <Boxes size={17} />
            Inventory
          </a>
          <a className="nav-item" href="#fulfillment">
            <Truck size={17} />
            Fulfillment
          </a>
          <a className="nav-item" href="#support">
            <Headphones size={17} />
            Support
          </a>
        </nav>
      </aside>

      <section className="dashboard" id="overview">
        <header className="topbar">
          <div>
            <p className="eyebrow">Operations dashboard</p>
            <h1>BusinessOps AI Assistant</h1>
          </div>

          <div className="topbar-actions">
            <span className={`api-pill ${health?.status === "ok" ? "ok" : ""}`}>
              <span className="status-dot" />
              {health?.status === "ok" ? "API online" : "API pending"}
            </span>
            <button className="refresh-button" onClick={loadDashboard} disabled={isLoading}>
              <RefreshCw size={17} className={isLoading ? "spin" : ""} />
              Refresh
            </button>
          </div>
        </header>

        <section className="summary-band" aria-label="Operations risk summary">
          <div>
            <p className="band-label">Current risk</p>
            <p className="risk-level">{riskLevel}</p>
          </div>
          <div className="band-copy">
            <span>
              {summary
                ? `${summary.delayedShipments} delayed shipments and ${summary.highPrioritySupportIssues} high-priority issues need attention.`
                : "Loading current operational signals from the API."}
            </span>
            <ArrowUpRight size={19} />
          </div>
        </section>

        {error ? (
          <section className="error-panel" role="alert">
            <AlertTriangle size={22} />
            <div>
              <h2>Dashboard data unavailable</h2>
              <p>{error}</p>
            </div>
          </section>
        ) : null}

        <section className="metrics-grid" aria-label="Operations metrics">
          {isLoading && metrics.length === 0
            ? Array.from({ length: 6 }, (_, index) => (
                <div className="metric-card skeleton" key={index}>
                  <span />
                  <strong />
                  <em />
                </div>
              ))
            : metrics.map((metric) => (
                <article className={`metric-card ${metric.tone}`} key={metric.label}>
                  <div className="metric-icon">
                    <metric.icon size={20} strokeWidth={2.2} />
                  </div>
                  <div>
                    <p>{metric.label}</p>
                    <strong>{metric.value}</strong>
                    <span>{metric.detail}</span>
                  </div>
                </article>
              ))}
        </section>

        <section className="workbench" aria-label="Operations focus areas">
          <div className="focus-column" id="inventory">
            <div className="section-title">
              <Boxes size={18} />
              <h2>Inventory</h2>
            </div>
            <p className="focus-value">{summary?.lowStockProducts ?? "--"}</p>
            <p className="focus-copy">Products are at or below reorder point.</p>
          </div>

          <div className="focus-column" id="fulfillment">
            <div className="section-title">
              <Clock3 size={18} />
              <h2>Fulfillment</h2>
            </div>
            <p className="focus-value">
              {summary ? summary.delayedShipments + summary.ordersMissingTracking : "--"}
            </p>
            <p className="focus-copy">Shipment records need timing or tracking cleanup.</p>
          </div>

          <div className="focus-column" id="support">
            <div className="section-title">
              <Headphones size={18} />
              <h2>Support</h2>
            </div>
            <p className="focus-value">{summary?.unresolvedSupportIssues ?? "--"}</p>
            <p className="focus-copy">Support issues remain unresolved.</p>
          </div>
        </section>
      </section>
    </main>
  );
}

export default App;
