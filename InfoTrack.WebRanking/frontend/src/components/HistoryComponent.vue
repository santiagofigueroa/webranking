<template>
    <div>
        <div v-if="isLoading">Loading...</div>
        <div v-if="error">{{ error }}</div>

        <table v-if="historyData.length" class="history-table">
            <thead>
                <tr>
                    <th>Keywords</th>
                    <th>URL</th>
                    <th>Result Positions</th>
                    <th>Search Date</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in historyData" :key="item.searchDate">
                    <td>{{ item.keywords || '-' }}</td>
                    <td>{{ item.url || '-' }}</td>
                    <td>{{ item.resultPositions || '-' }}</td>
                    <td>{{ item.searchDate }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>


<script>
import API from '../services/api';

export default {
    data() {
        return {
            historyData: [],
            isLoading: true,
            error: null
        }
    },

methods: {
    fetchData() {
        this.isLoading = true;
        this.error = null;

        API.getSearchHistory()
            .then(response => {
                this.historyData = response;
                this.isLoading = false;
            })
            .catch(error => {
                this.error = "There was an error fetching the history data.";
                this.isLoading = false;
                console.error(error);
            });
    }
},
mounted() {
    this.fetchData();
}
}
</script>
<style scoped>
.history-table {
    width: 100%;
    border-collapse: collapse;
    margin-top: 20px;
}

.history-table th, .history-table td {
    padding: 10px 15px;
    border: 1px solid #ddd;
}

.history-table thead {
    background-color: #f2f2f2;
}

.history-table tr:hover {
    background-color: #f5f5f5;
}

.history-table td {
    vertical-align: top;
}
</style>