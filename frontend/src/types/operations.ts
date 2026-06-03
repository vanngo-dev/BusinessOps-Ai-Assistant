export type OperationsSummary = {
  totalProducts: number;
  lowStockProducts: number;
  openOrders: number;
  delayedShipments: number;
  ordersMissingTracking: number;
  highPrioritySupportIssues: number;
  unresolvedSupportIssues: number;
  estimatedOpenOrderValue: number;
};

export type HealthStatus = {
  status: string;
  app: string;
};
