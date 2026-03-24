<script setup>
import { watch, onMounted } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();

const updateBodyClass = (name) => {
  document.body.className = "";
  if (name) document.body.classList.add(name);
};

onMounted(() => updateBodyClass(route.name));

watch(
  () => route.name,
  (newName) => updateBodyClass(newName),
);
</script>

<template>
  <router-view />
</template>

<style scoped></style>
