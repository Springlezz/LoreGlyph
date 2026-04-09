<script setup>
import { ref } from 'vue'
import { authService } from '@/services/authService'
import { useToast } from "vue-toastification";

const toast = useToast();
const userName = ref('')
const login = ref('')
const password = ref('')
const secretWord = ref('')

const emit = defineEmits(['close'])

const register = async () => {
  try {
    await authService.register({
      userName: userName.value,
      login: login.value,
      password: password.value,
      secretWord: secretWord.value
    })
    if (!userName.value || !login.value || !password.value || !secretWord.value) {
      toast.error('Заполните все поля')
      return
    }
    toast.success('Регистрация прошла успешно')
    emit('close')
  } catch (e) {
    toast.error(e.response?.data || 'Ошибка регистрации')
  }
}
</script>

<template>
  <div class="modal">
    <h2>Регистрация</h2>

    <input v-model="userName" placeholder="Имя" />
    <input v-model="login" placeholder="Логин" />
    <input v-model="password" type="password" placeholder="Пароль" />
    <input v-model="secretWord" placeholder="Кодовое слово" />

    <button @click="register">Создать</button>
    <button @click="emit('close')">Закрыть</button>
  </div>
</template>