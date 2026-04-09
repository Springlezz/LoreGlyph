<template>
  <header class="header">
    <div
      class="left-header"
      @click="$router.push('/languages')"
      title="Вернуться на главную страницу"
    >
      <img class="logo" src="../assets/cube_black_logo.svg" alt="logo" />
      <h1>LoreGlyph</h1>
    </div>
    <div class="right-header">
      <button @click="$router.push('/languages')" class="right-header-buttons">
        Вернуться
      </button>
      <p>|</p>
      <button @click="logout" class="right-header-buttons">Выйти</button>
    </div>
  </header>
  <main>
    <div class="center-information">
      <img class="avatar-default" />
      <div class="circle"></div>
      <h1 class="title">{{ userName }}</h1>
      <p class="information">Дата регистрации: {{ createdBy }}</p>
    </div>
    <div class="sections">
      <div class="left-sections">
        <h1 class="title">Информация</h1>
        <p class="information">{{ userName }}</p>
        <p class="information">{{ login }}</p>
      </div>
      <div class="right-sections">
        <h1 class="title">Смена пароля</h1>
        <input v-model="oldPassword" placeholder="Введите старый пароль" />
        <input v-model="newPassword" placeholder="Введите новый пароль" />
      </div>
    </div>
    <div class="sections">
      <button @click="deleteAccount" class="warning-button">
        Удалить аккаунт
      </button>
      <button @click="changePassword" class="save-changes-button">
        Сохранить изменения
      </button>
    </div>
  </main>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { userService } from "@/services/userService";
import { authService } from "@/services/authService";
import { useToast } from "vue-toastification";

const toast = useToast();

const userName = ref("");
const login = ref("");
const createdBy = ref("");

const oldPassword = ref("");
const newPassword = ref("");

const changePassword = async () => {
  try {
    await authService.resetPassword({
      Login: login.value,
      OldPassword: oldPassword.value,
      NewPassword: newPassword.value,
    });

    toast.success("Пароль изменён");
  } catch (e) {
    toast.error(e.response?.data || "Ошибка");
  }
};

const logout = () => {
  localStorage.clear();
  window.location.href = "/home";
};

const getUserIdFromToken = () => {
  const token = localStorage.getItem("token");
  try {
    const payload = JSON.parse(atob(token.split(".")[1]));

    const userId =
      payload[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
      ];
    return userId;
  } catch (e) {
    return null;
  }
};

const deleteAccount = async () => {
  if (!confirm("Удалить профиль. Вы не сможете его восстановить")) return;
  const userId = getUserIdFromToken();

  try {
    await userService.deleteAccount(userId);
    localStorage.clear();
    window.location.href = "/home";
  } catch (e) {
    toast.error("Ошибка при удалении аккаунта");
  }
};

onMounted(async () => {
  try {
    const res = await userService.getMe();
    const data = res.data;

    userName.value = data.userName;
    login.value = data.login;
    createdBy.value = new Date(data.createdBy).toLocaleDateString();
  } catch (e) {
    toast.error("Ошибка загрузки профиля:", e);
  }
});
</script>

<style scoped>
.save-changes-button {
  color: var(--green);
  border: none;
  font-family: "Montserrat-Bold", sans-serif;
  background: none;
  font-size: 1.5rem;
  padding: 1rem 1rem;
  border-radius: 2rem;
  box-shadow:
    rgba(0, 0, 0, 0.25) 0px 0.0625em 0.0625em,
    rgba(0, 0, 0, 0.25) 0px 0.125em 0.5em,
    rgba(255, 255, 255, 0.1) 0px 0px 0px 1px inset;
  cursor: pointer;
}

.warning-button {
  color: var(--red);
  border: none;
  font-family: "Montserrat-Bold", sans-serif;
  background: none;
  font-size: 1.5rem;
  padding: 1rem 1rem;
  border-radius: 2rem;
  box-shadow:
    rgba(0, 0, 0, 0.25) 0px 0.0625em 0.0625em,
    rgba(0, 0, 0, 0.25) 0px 0.125em 0.5em,
    rgba(255, 255, 255, 0.1) 0px 0px 0px 1px inset;
  cursor: pointer;
}
input {
  font-family: "Montserrat-Regular", sans-serif;
  font-size: 1.5rem;
  margin: 1rem;
  padding: 10px 10px 10px 5px;
  display: block;
  width: 500px;
  border: none;
  border-bottom: 1px solid var(--middle-dark-gray);
}

input:focus {
  outline: none;
}

.circle {
  width: 25rem;
  height: 25rem;
  border-radius: 50%;
  background: var(--middle-dark-gray);
}

.sections {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 2rem;
  width: 100%;
  margin-top: 2rem;
}

.center-information {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
}

main {
  padding: 3rem 7.5rem;
}

.title {
  font-family: "Montserrat-Bold", sans-serif;
  font-size: 2.5rem;
  text-align: center;
}

.information {
  font-family: "Montserrat-Regular", sans-serif;
  font-size: 1.5rem;
  margin: 1rem;
  padding: 10px 10px 10px 5px;
  display: block;
  width: 500px;
  border: none;
  color: var(--middle-dark-gray);
  border-bottom: 1px solid var(--middle-dark-gray);
}
</style>
