<template>
  <div class="modal">
    <h2>Создание языка</h2>

    <input v-model="name" placeholder="Название" />
    <input v-model="description" placeholder="Описание" />

    <button @click="createLanguage">Создать</button>
    <button @click="() => emit('close')">Закрыть</button>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { languageService } from "@/services/languageService";

const name = ref("");
const description = ref("");

const emit = defineEmits(["close", "created"]);

const createLanguage = async () => {
  if (!name.value || !description.value) {
    alert("Заполните все поля");
    return;
  }

  try {
    await languageService.create({
      name: name.value,
      description: description.value,
    });

    name.value = "";
    description.value = "";

    emit("created");
    emit("close");
  } catch (e) {
    alert(e.response?.data || "Ошибка создания");
  }
};
</script>
