import { SortField } from "./SortField";
import { SortOrder } from "./SortOrder";

export interface Filters {
    startYear: number | null;
    endYear: number | null;
    recClass: string | null;
    name: string;
    sortField: SortField | SortField.Year,
    sortOrder: SortOrder | SortOrder.Asc
};