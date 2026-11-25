import { Transaction } from './transaction';

export interface PagedTransactions {
  items: Transaction[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
