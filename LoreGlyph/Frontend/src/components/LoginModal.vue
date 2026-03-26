<script setup>
import { ref } from "vue";
import { authService } from "@/services/authService";
import { useRouter } from "vue-router";

const login = ref("");
const password = ref("");

const emit = defineEmits(["close", "openRegister", "openReset"]);

const router = useRouter();
const loginUser = async () => {
  try {
    const res = await authService.login({
      login: login.value,
      password: password.value,
    });

    console.log("Успешный вход", res);

    emit("close");
    router.push("/languages");
  } catch (e) {
    alert(e.response?.data || "Ошибка входа");
  }
};
</script>

<template>
  <div class="modal">
    <h2>Вход</h2>

    <input v-model="login" placeholder="Логин" />
    <input v-model="password" type="password" placeholder="Пароль" />

    <button @click="loginUser">Войти</button>

    <br /><br />

    <button @click="emit('openRegister')">Регистрация</button>

    <button @click="emit('openReset')">Забыли пароль?</button>

    <br /><br />

    <button class="modal-button" @click="emit('close')">Закрыть</button>
  </div>
</template>

<style scoped></style>
