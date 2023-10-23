<template>
    <div>
        <h3>Search History</h3>
        <ul>
            <li v-for="history in searchHistories" :key="history.id">
                {{ history.Keywords }} - {{ history.SearchEngineTitle }}: {{ history.Ranking }}
            </li>
        </ul>
    </div>
</template>

<script>
import API from '../services/api';

export default {
    data() {
        return {
            searchHistories: []
        }
    },
    async created() {
        try {
            let response = await API.getSearchHistory();
            this.searchHistories = response.data;
        } catch (error) {
            console.error("Failed to load search history:", error.message);
        }
    }
}
</script>
