import { useEffect, useMemo, useState } from "react";
import type { ComponentType, CSSProperties } from "react";
import {
  AlertTriangle,
  BarChart3,
  Boxes,
  CircleDollarSign,
  ClipboardList,
  Gauge,
  PackageCheck,
  PackageSearch,
  RefreshCw,
  ShieldCheck,
  Ticket,
  Truck
} from "lucide-react";
import { getOperationsSummary } from "../api/businessOpsApi";
import type { OperationsSummary } from "../types/operations";
import "./OperationsDashboard.css";

type Tone = "teal" | "amber" | "red" | "green" | "blue" | "gray";

type MetricCard = {
  label: string;
  value: string;
  note: string;
  tone: Tone;
  icon: ComponentType<{ size?: number; strokeWidth?: number }>;
};

type RiskAssessment = {
  label: "Stable" | "Watch" | "Elevated";
  score: number;
  tone: Tone;
  summary: string;
};

type PriorityAction = {
  label: string;
  value: string;
  tone: Tone;
};

type RiskBar = {
  label: string;
  value: number;
  total: number;
  tone: Tone;
};

const currencyFormatter = new Intl.NumberFormat("en-US", {
  style: "currency",
  currency: "USD",
  maximumFractionDigits: 0
});

function formatCurrency(value: number) {
  return currencyFormatter.format(value);
}

function getRiskAssessment(summary: OperationsSummary): RiskAssessment {
  const score =
    summary.lowStockProducts +
    summary.delayedShipments * 2 +
    summary.ordersMissingTracking +
    summary.highPrioritySupportIssues * 2 +
    Math.ceil(summary.unresolvedSupportIssues / 2);

  if (score >= 16) {
    return {
      label: "Elevated",
      score,
      tone: "red",
      summary: "Fulfillment and support pressure are above the normal operating range."
    };
  }

  if (score >= 8) {
    return {
      label: "Watch",
      score,
      tone: "amber",
      summary: "Several operating signals need review before they compound."
    };
  }

  return {
    label: "Stable",
    score,
    tone: "green",
    summary: "Current operating signals are within a manageable range."
  };
}

function getPriorityActions(summary: OperationsSummary): PriorityAction[] {
  const actions: PriorityAction[] = [];

  if (summary.lowStockProducts > 0) {
    actions.push({
      label: "Review reorder queue",
      value: `${summary.lowStockProducts} low-stock products`,
      tone: "amber"
    });
  }

  if (summary.delayedShipments > 0) {
    actions.push({
      label: "Escalate fulfillment delays",
      value: `${summary.delayedShipments} delayed shipments`,
      tone: "red"
    });
  }

  if (summary.ordersMissingTracking > 0) {
    actions.push({
      label: "Assign missing tracking",
      value: `${summary.ordersMissingTracking} orders missing tracking`,
      tone: "amber"
    });
  }

  if (summary.highPrioritySupportIssues > 0) {
    actions.push({
      label: "Triage priority support",
      value: `${summary.highPrioritySupportIssues} high-priority issues`,
      tone: "red"
    });
  }

  if (actions.length === 0) {
    actions.push({
      label: "Monitor daily operations",
      value: "No urgent exceptions",
      tone: "green"
    });
  }

  return actions;
}

function getMetrics(summary: OperationsSummary): MetricCard[] {
  return [
    {
      label: "Open order value",
      value: formatCurrency(summary.estimatedOpenOrderValue),
      note: "Revenue tied to active orders",
      tone: "teal",
      icon: CircleDollarSign
    },
    {
      label: "Open orders",
      value: summary.openOrders.toString(),
      note: "Orders in open or processing status",
      tone: summary.openOrders > 0 ? "blue" : "green",
      icon: ClipboardList
    },
    {
      label: "Total products",
      value: summary.totalProducts.toString(),
      note: "Active catalog records in scope",
      tone: "gray",
      icon: PackageCheck
    },
    {
      label: "Low-stock products",
      value: summary.lowStockProducts.toString(),
      note: "At or below reorder point",
      tone: summary.lowStockProducts > 0 ? "amber" : "green",
      icon: Boxes
    },
    {
      label: "Delayed shipments",
      value: summary.delayedShipments.toString(),
      note: "Late, delayed, or blocked shipments",
      tone: summary.delayedShipments > 0 ? "red" : "green",
      icon: Truck
    },
    {
      label: "Missing tracking",
      value: summary.ordersMissingTracking.toString(),
      note: "Undelivered shipments without tracking",
      tone: summary.ordersMissingTracking > 0 ? "amber" : "green",
      icon: PackageSearch
    },
    {
      label: "High-priority issues",
      value: summary.highPrioritySupportIssues.toString(),
      note: "Priority customer issues unresolved",
      tone: summary.highPrioritySupportIssues > 0 ? "red" : "green",
      icon: AlertTriangle
    },
    {
      label: "Unresolved support",
      value: summary.unresolvedSupportIssues.toString(),
      note: "Open support workload",
      tone: summary.unresolvedSupportIssues > 0 ? "blue" : "green",
      icon: Ticket
    }
  ];
}

function getRiskBars(summary: OperationsSummary): RiskBar[] {
  const inventoryRisk = summary.lowStockProducts;
  const fulfillmentRisk = summary.delayedShipments + summary.ordersMissingTracking;
  const supportRisk = summary.unresolvedSupportIssues;
  const largest = Math.max(inventoryRisk, fulfillmentRisk, supportRisk, 1);

  return [
    {
      label: "Inventory",
      value: inventoryRisk,
      total: largest,
      tone: inventoryRisk > 0 ? "amber" : "green"
    },
    {
      label: "Fulfillment",
      value: fulfillmentRisk,
      total: largest,
      tone: summary.delayedShipments > 0 ? "red" : fulfillmentRisk > 0 ? "amber" : "green"
    },
    {
      label: "Support",
      value: supportRisk,
      total: largest,
      tone: summary.highPrioritySupportIssues > 0 ? "red" : "blue"
    }
  ];
}

function getBarStyle(bar: RiskBar): CSSProperties {
  const width = bar.total === 0 ? 0 : Math.max((bar.value / bar.total) * 100, bar.value > 0 ? 8 : 0);

  return {
    "--bar-width": `${Math.min(width, 100)}%`
  } as CSSProperties;
}

function OperationsDashboard() {
  const [summary, setSummary] = useState<OperationsSummary | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [lastUpdated, setLastUpdated] = useState<string | null>(null);

  async function loadSummary() {
    try {
      setIsLoading(true);
      setError(null);

      const result = await getOperationsSummary();

      setSummary(result);
      setLastUpdated(new Date().toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" }));
    } catch (err) {
      setError(err instanceof Error ? err.message : "Unable to load operations summary");
    } finally {
      setIsLoading(false);
    }
  }

  useEffect(() => {
    void loadSummary();
  }, []);

  const risk = useMemo(() => (summary ? getRiskAssessment(summary) : null), [summary]);
  const metrics = useMemo(() => (summary ? getMetrics(summary) : []), [summary]);
  const actions = useMemo(() => (summary ? getPriorityActions(summary) : []), [summary]);
  const riskBars = useMemo(() => (summary ? getRiskBars(summary) : []), [summary]);

  return (
    <main className="operations-page">
      <section className="dashboard-shell" aria-label="Operations dashboard">
        <header className="dashboard-header">
          <div className="brand-row">
            <span className="brand-icon" aria-hidden="true">
              <BarChart3 size={22} strokeWidth={2.4} />
            </span>
            <div>
              <p className="eyebrow">BusinessOps AI Assistant</p>
              <h1>Operations Dashboard</h1>
            </div>
          </div>

          <div className="header-actions">
            <span className={`status-pill ${error ? "error" : summary ? "ready" : ""}`}>
              <span className="status-dot" />
              {error ? "Data unavailable" : summary ? "Live API data" : "Loading data"}
            </span>
            <button className="refresh-button" onClick={loadSummary} disabled={isLoading}>
              <RefreshCw size={17} className={isLoading ? "spin" : ""} />
              Refresh
            </button>
          </div>
        </header>

        <section className="risk-panel" aria-label="Current operations risk">
          <div className="risk-score-block">
            <p className="section-kicker">Current risk</p>
            <p className={`risk-label ${risk?.tone ?? "gray"}`}>{risk?.label ?? "Loading"}</p>
            <p className="risk-copy">
              {risk?.summary ?? "Loading current operating signals from the insights API."}
            </p>
          </div>

          <div className="risk-score-card">
            <Gauge size={24} strokeWidth={2.4} />
            <div>
              <span>Risk score</span>
              <strong>{risk?.score ?? "--"}</strong>
            </div>
          </div>

          <div className="risk-score-card">
            <ShieldCheck size={24} strokeWidth={2.4} />
            <div>
              <span>Last refreshed</span>
              <strong>{lastUpdated ?? "--"}</strong>
            </div>
          </div>
        </section>

        {error ? (
          <section className="error-panel" role="alert">
            <AlertTriangle size={22} />
            <div>
              <h2>Operations summary unavailable</h2>
              <p>{error}</p>
            </div>
          </section>
        ) : null}

        <section className="metric-grid" aria-label="Operations metrics">
          {isLoading && metrics.length === 0
            ? Array.from({ length: 8 }, (_, index) => (
                <article className="metric-card skeleton" key={index}>
                  <span />
                  <strong />
                  <em />
                </article>
              ))
            : metrics.map((metric) => (
                <article className={`metric-card ${metric.tone}`} key={metric.label}>
                  <div className="metric-top">
                    <span className="metric-icon">
                      <metric.icon size={20} strokeWidth={2.2} />
                    </span>
                    <p>{metric.label}</p>
                  </div>
                  <strong>{metric.value}</strong>
                  <span>{metric.note}</span>
                </article>
              ))}
        </section>

        <section className="operations-workbench" aria-label="Operations workbench">
          <article className="priority-panel">
            <div className="panel-heading">
              <AlertTriangle size={18} />
              <h2>Priority Queue</h2>
            </div>
            <div className="action-list">
              {actions.map((action) => (
                <div className={`action-row ${action.tone}`} key={action.label}>
                  <span />
                  <div>
                    <strong>{action.label}</strong>
                    <p>{action.value}</p>
                  </div>
                </div>
              ))}
            </div>
          </article>

          <article className="risk-mix-panel">
            <div className="panel-heading">
              <Gauge size={18} />
              <h2>Risk Mix</h2>
            </div>
            <div className="risk-bars">
              {riskBars.map((bar) => (
                <div className="risk-bar-row" key={bar.label}>
                  <div className="risk-bar-label">
                    <span>{bar.label}</span>
                    <strong>{bar.value}</strong>
                  </div>
                  <div className={`risk-bar-track ${bar.tone}`} style={getBarStyle(bar)}>
                    <span />
                  </div>
                </div>
              ))}
            </div>
          </article>
        </section>
      </section>
    </main>
  );
}

export default OperationsDashboard;
