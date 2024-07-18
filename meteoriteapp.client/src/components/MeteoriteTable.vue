<template>
    <div class="container">
        <div class="filters">
            <div class="filter-item">
                <label>
                    Start Year:
                    <div class="input-container">
                        <select v-model="filters.startYear">
                            <option :value="null">Not selected</option>
                            <option v-for="year in filteredStartYears" :key="year" :value="year">{{ year }}</option>
                        </select>
                        <span v-if="filters.startYear" @click="clearStartYear" class="clear-icon">&times;</span>
                    </div>
                </label>
            </div>
            <div class="filter-item">
                <label>
                    End Year:
                    <div class="input-container">
                        <select v-model="filters.endYear">
                            <option :value="null">Not selected</option>
                            <option v-for="year in filteredEndYears" :key="year" :value="year">{{ year }}</option>
                        </select>
                        <span v-if="filters.endYear" @click="clearEndYear" class="clear-icon">&times;</span>
                    </div>
                </label>
            </div>
            <div class="filter-item">
                <label>
                    Meteorite Class:
                    <div class="input-container">
                        <select v-model="filters.recClass">
                            <option :value="null">Not selected</option>
                            <option v-for="recClass in recClasses" :key="recClass" :value="recClass">{{ recClass }}</option>
                        </select>
                        <span v-if="filters.recClass" @click="clearRecClass" class="clear-icon">&times;</span>
                    </div>
                </label>
            </div>
            <div class="filter-item">
                <label>
                    Name:
                    <div class="input-container">
                        <input type="text" v-model="filters.name" />
                        <span v-if="filters.name" @click="clearName" class="clear-icon">&times;</span>
                    </div>
                </label>
            </div>
            <div class="filter-item">
                <button @click="fetchDataClick" style="height: 100%">Fetch Data</button>
            </div>
            <div class="filter-item">
                <button @click="clearAll" style="height: 100%">Clear all filters</button>
            </div>
        </div>
        <div class="pagination-controls">
            <div class="pagination-item">
                <label>
                    Items per page:
                    <select v-model="itemsPerPage">
                        <option :value="25">25</option>
                        <option :value="50">50</option>
                        <option :value="100">100</option>
                    </select>
                </label>
            </div>
            <div class="pagination-item">
                <button @click="prevPage" :disabled="currentPage === 1">Previous</button>
                <span>Page {{ currentPage }} of {{ totalPages }}</span>
                <button @click="nextPage" :disabled="currentPage === totalPages">Next</button>
            </div>
        </div>
        <div class="sort-controls">
            <label>
                Sort by:
                <select v-model="filters.sortField">
                    <option :value="0">Year</option>
                    <option :value="1">Count</option>
                    <option :value="2">Total Mass</option>
                </select>
            </label>
            <label>
                Order:
                <select v-model="filters.sortOrder">
                    <option :value="0">Ascending</option>
                    <option :value="1">Descending</option>
                </select>
            </label>
        </div>
        <table class="meteorite-table">
            <thead>
                <tr>
                    <th @click="setSortField(SortField.Year)">Year</th>
                    <th @click="setSortField(SortField.Count)">Count</th>
                    <th @click="setSortField(SortField.TotalMass)">Total Mass</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="group in sortedMeteoriteGroups" :key="group.year">
                    <td width="30%">{{ group.year }}</td>
                    <td width="30%">{{ group.count }}</td>
                    <td width="40%">{{ group.totalMass }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import api from '../services/api';
    import { Filters } from '../models/Filters';
    import { MeteoriteGroup } from '../models/MeteoriteGroup';
    import { PagedResponse } from '../models/PagedResponse';
    import { SortField } from '../models/SortField';
    import { SortOrder } from '../models/SortOrder';

    export default defineComponent({
        data() {
            return {
                filters: {
                    startYear: null,
                    endYear: null,
                    recClass: null,
                    name: '',
                    sortField: SortField.Year,
                    sortOrder: SortOrder.Asc
                } as Filters,
                meteoriteGroups: [] as MeteoriteGroup[],
                years: [] as number[],
                recClasses: [] as string[],
                currentPage: 1,
                itemsPerPage: 25,
                totalCount: 0
            };
        },
        computed: {
            filteredEndYears() {
                if (this.filters.startYear) {
                    return this.years.filter(year => year >= this.filters.startYear);
                }
                return this.years;
            },
            filteredStartYears() {
                if (this.filters.endYear) {
                    return this.years.filter(year => year <= this.filters.endYear);
                }
                return this.years;
            },
            sortedMeteoriteGroups() {
                return [...this.meteoriteGroups].sort((a, b) => {
                    const fieldA = a[this.sortField];
                    const fieldB = b[this.sortField];

                    if (this.sortOrder === SortOrder.Asc) {
                        return fieldA > fieldB ? 1 : fieldA < fieldB ? -1 : 0;
                    } else {
                        return fieldA < fieldB ? 1 : fieldA > fieldB ? -1 : 0;
                    }
                });
            },
            totalPages() {
                return Math.ceil(this.totalCount / this.itemsPerPage);
            }
        },
        async created() {
            await this.fetchYears();
            await this.fetchClasses();
            await this.fetchData();
        },
        methods: {
            async fetchData() {
                try {
                    const response = await api.getMeteoritePagedGroups(this.filters, this.currentPage, this.itemsPerPage);
                    this.meteoriteGroups = response.data.items;
                    this.totalCount = response.data.totalCount;
                } catch (error) {
                    console.error('Error fetching data:', error);
                }
            },
            async fetchDataClick() {
                this.currentPage = 1;
                await this.fetchData();
            },
            async fetchYears() {
                try {
                    const response = await api.getYears();
                    this.years = response.data;
                } catch (error) {
                    console.error('Error fetching years:', error);
                }
            },
            async fetchClasses() {
                try {
                    const response = await api.getClasses();
                    this.recClasses = response.data;
                } catch (error) {
                    console.error('Error fetching classes:', error);
                }
            },
            setSortField(field: SortField) {
                if (this.filters.sortField === field) {
                    this.filters.sortOrder = this.filters.sortOrder === SortOrder.Asc ? SortOrder.Desc : SortOrder.Asc;
                } else {
                    this.filters.sortField = field;
                    this.filters.sortOrder = SortOrder.Asc;
                }
            },
            async prevPage() {
                if (this.currentPage > 1) {
                    this.currentPage--;
                    await this.fetchData();
                }
            },
            async nextPage() {
                if (this.currentPage < this.totalPages) {
                    this.currentPage++;
                    await this.fetchData();
                }
            },
            clearStartYear() {
                this.filters.startYear = null;
            },
            clearEndYear() {
                this.filters.endYear = null;
            },
            clearRecClass() {
                this.filters.recClass = null;
            },
            clearName() {
                this.filters.name = '';
            },
            clearAll() {
                this.clearStartYear();
                this.clearEndYear();
                this.clearRecClass();
                this.clearName();
            }
        },
        watch: {
            'filters.startYear'(newStartYear) {
                if (this.filters.endYear && newStartYear && newStartYear > this.filters.endYear) {
                    this.filters.endYear = newStartYear;
                }
            },
            'filters.endYear'(newEndYear) {
                if (this.filters.startYear && newEndYear && newEndYear < this.filters.startYear) {
                    this.filters.startYear = newEndYear;
                }
            }
        }
    });
</script>

<style scoped>
    .container {
        width: 100%;
        margin: 0 auto;
    }

    .filters {
        display: flex;
        flex-wrap: wrap;
        gap: 10px;
        margin-bottom: 20px;
    }

    .filter-item {
        flex: 1 1 200px;
        display: flex;
        flex-direction: column;
    }

        .filter-item label {
            display: flex;
            flex-direction: column;
        }

    .input-container {
        position: relative;
        display: flex;
        align-items: center;
    }

        .input-container select,
        .input-container input {
            flex: 1;
            padding-right: 24px; /* Space for the clear icon */
        }

    .clear-icon {
        position: absolute;
        right: 20px;
        cursor: pointer;
    }

    .pagination-controls {
        display: flex;
        flex-wrap: wrap;
        gap: 10px;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 20px;
    }

    .pagination-item {
        display: flex;
        align-items: center;
        gap: 10px;
    }

    .meteorite-table {
        width: 100%;
        border-collapse: collapse;
    }

    th {
        cursor: pointer;
        background-color: #f9f9f9;
    }

    th, td {
        border: 1px solid #ddd;
        padding: 8px;
        text-align: left;
    }

        th:hover {
            background-color: #f1f1f1;
        }
</style>
