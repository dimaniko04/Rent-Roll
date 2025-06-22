export type PaginatedList<T> = {
  currentPage: number;
  pageSize: number;
  totalCount: number;
  items: T[];
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
};
