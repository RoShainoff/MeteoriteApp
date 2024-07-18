import axios from 'axios';
import { AxiosResponse } from 'axios';
import { Filters } from './models/Filters';
import { MeteoriteGroup } from './models/MeteoriteGroup';
import { PagedResponse } from './models/PagedResponse';

const apiClient = axios.create({
    baseURL: 'https://localhost:44305/api',
    withCredentials: false,
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
    }
});

export default {
    getMeteoritePagedGroups(filters: Filters, pageNumber: number, pageSize: number): Promise<AxiosResponse<PagedResponse<MeteoriteGroup>>> {
        return apiClient.get < PagedResponse < MeteoriteGroup >> ('/meteorite/pagedFilteredGroups', {
            params: {
                startYear: filters.startYear,
                endYear: filters.endYear,
                recClass: filters.recClass,
                name: filters.name,
                sortField: filters.sortField,
                sortOrder: filters.sortOrder,
                pageNumber,
                pageSize
            }
        });
    },
    getYears(): Promise<AxiosResponse<number[]>> {
        return apiClient.get < number[] > ('/meteorite/years');
    },
    getClasses(): Promise<AxiosResponse<string[]>> {
        return apiClient.get < string[] > ('/meteorite/classes');
    }
};
