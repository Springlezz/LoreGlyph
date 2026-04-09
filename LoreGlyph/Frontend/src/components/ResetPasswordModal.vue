<script setup>
import { ref } from 'vue'
import { userService } from '@/services/userService'
import { useToast } from "vue-toastification";

const toast = useToast();

const login = ref('')
const secretWord = ref('')
const newPassword = ref('')

const emit = defineEmits(['close'])

const reset = async () => {
  try {
    await userService.resetPassword({
      login: login.value,
      secretWord: secretWord.value,
      newPassword: newPassword.value
    })

    toast.success('Пароль сброшен')
    emit('close')
  } catch (e) {
    toast.success(e.response?.data || 'Ошибка сброса')
  }
}
</script>

<template>
  <div class="modal">
    <h3>Сброс пароля</h3>

    <input v-model="login" placeholder="Логин" />
    <input v-model="secretWord" placeholder="Кодовое слово" />
    <input v-model="newPassword" type="password" placeholder="Новый пароль" />

    <button @click="reset">Сбросить</button>
    <button @click="emit('close')">Закрыть</button>
  </div>
</template>