import axios from "axios";

class ApiService {
  constructor() {
    this.API_URL = this.getApiUrl(); // Get API URL based on environment
  }

  getApiUrl() {
    if (process.env.NODE_ENV === "production") {
      return "NOT Implemented"; // In case we expand our application live    
    } else {
      return "http://localhost:5148/search/"; // DEV environment URL
    }
  }

  async submitSearch(searchData) {
    try {
      const response = await axios.post(this.API_URL + "search", searchData);
      console.log(response);
      return response.data; 
    } catch (error) {
      console.error("Error posting search data:", error.response ? error.response.data : error.message);
      throw error;
    }
  }

  // Method to get the list of search engines from the GetSearchEngines endpoint
  async getSearchEngines() {
    try {
      const response = await axios.get(this.API_URL + "getsearchengines"); 
      console.log(response.data);
      return response.data; // This should return the data directly
    } catch (error) {
      console.error("Error fetching search engines:", error.response ? error.response.data : error.message);
      throw error;
    }
}

async getSearchHistory() {
    try {
        const response = await axios.get(this.API_URL + "getsearchhistory");
        console.log(response);
        return response.data;
    } catch (error) {
        console.error("Error fetching search history:", error.response ? error.response.data : error.message);
        throw error;
    }
}

}

export default new ApiService();
