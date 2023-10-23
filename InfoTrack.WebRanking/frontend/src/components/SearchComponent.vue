<template>
    <div class="web-ranking">
        <h3>Web Ranking</h3>
        <div class="input-group">
            <label for="keywords">Keywords:</label>
            <input v-model="form.Keywords" id="keywords" type="text" />
        </div>

        <div class="input-group">
            <label for="searchEngine">Search Engine:</label>
            <select v-model="form.SelectedSearchEngineId" id="searchEngine">
                <option v-for="engine in form.AvailableSearchEngines" :key="engine.id" :value="engine.id">
                    {{ engine.title }}
                </option>
            </select>
        </div>

        <button class="search-btn" @click="submitForm">Search</button>

        <div class="results" v-if="form.resultPositions">
            <h4>Search Results</h4>
            <p><strong>Keywords:</strong> {{ form.keywords }}</p>
            <p><strong>URL:</strong> {{ form.url }}</p>
            <p><strong>Position:</strong> {{ form.resultPositions }}</p>
            <p><strong>Search Engine:</strong> {{ getEngineNameById(form.SelectedSearchEngineId) }}</p>
        </div>
    </div>

</template>

<script>
    import API from '../services/api';

    export default {
        data() {
            return {
                form: {
                    Keywords: '',
                    Url: '',
                    ResultPositions: '',
                    SelectedSearchEngineId: null,
                    AvailableSearchEngines: []
                }
            }
        },
        async created() {
            try {
                let response = await API.getSearchEngines();
                console.log(response);
                this.form.AvailableSearchEngines = response;
            } catch (error) {
                console.error("Failed to load available search engines:", error.message);
            }
        },
        methods: {
            getEngineNameById(id) {
                const engine = this.form.availableSearchEngines.find(e => e.id === id);
                return engine ? engine.title : '';
            },
            submitForm() {
                console.log(this.form);
                API.submitSearch(this.form)
                    .then((res) => {
                        console.log(res);
                        this.form = res;
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            },
            resetForm() {
                this.form = {
                    Keywords: '',
                    Url: '',
                    ResultPositions: '',
                    SelectedSearchEngineId: null,
                    AvailableSearchEngines: this.form.AvailableSearchEngines
                };
            }
        },
        mounted() {
        }
    }
</script>
<style scoped>
    .results {
        margin-top: 30px;
        border-top: 1px solid #ccc;
        padding-top: 15px;
    }

    .web-ranking {
        font-family: Arial, sans-serif;
        background-color: #f8f8f8;
        padding: 20px;
        border-radius: 10px;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
        width: 300px;
        margin: 20px auto;
    }

    .input-group {
        display: flex;
        flex-direction: column;
        margin-bottom: 15px;
    }

    label {
        margin-bottom: 5px;
        font-weight: bold;
        color: #333;
    }

    input, select {
        padding: 10px;
        border: 1px solid #ccc;
        border-radius: 5px;
        font-size: 14px;
    }

    button.search-btn {
        background-color: #007bff;
        color: white;
        border: none;
        padding: 10px 20px;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.2s ease-in-out;
    }

        button.search-btn:hover {
            background-color: #0056b3;
        }
</style>