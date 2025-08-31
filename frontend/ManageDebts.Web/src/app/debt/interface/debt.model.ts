export interface Debt {
  id: string;
  userId: string;
  debtorName: string;
  creditorId: string;
  creditorName: string;
  amount: number;
  description: string;
  isPaid: boolean;
  createdAt: string;
  paidAt: string | null;
}
