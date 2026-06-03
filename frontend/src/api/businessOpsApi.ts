import type { OperationsSummary } from "../types/operations";

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5074";

async function getJson<T>(path: string): Promise<T> {
  const response = await fetch(`${apiBaseUrl}${path}`);

  if (!response.ok) {
    throw new Error(`Request failed with status ${response.status}`);
  }

  return response.json() as Promise<T>;
}

export function getOperationsSummary(): Promise<OperationsSummary> {
  return getJson<OperationsSummary>("/api/insights/operations-summary");
}
