<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <Toast />

  <header>
    <nav>
      <RouterLink to="/">Home</RouterLink>
      <br />
      <RouterLink to="/sentiment">Sentiment</RouterLink>
    </nav>
  </header>

  <RouterView />
</template>
