<template>
    <nav>
      <ul>
        <li><router-link to="/">Search</router-link></li>
        <li><router-link to="/history">History</router-link></li>
      </ul>
    </nav>
  </template>
  
  <style scoped>
  nav {
      background-color: #333;
      padding: 0.5rem 1rem;
  }
  
  nav ul {
      list-style-type: none;
      margin: 0;
      padding: 0;
      display: flex;
      gap: 1rem;
  }
  
  nav li {
      display: inline;
  }
  
  nav a {
      text-decoration: none;
      color: white;
      padding: 0.5rem 1rem;
      border-radius: 0.5rem;
      transition: background-color 0.3s;
  }
  
  nav a:hover {
      background-color: #555;
  }
  </style>
  <script>
  export default {
    name: 'AppNavbar',

  }
  </script>
  
  