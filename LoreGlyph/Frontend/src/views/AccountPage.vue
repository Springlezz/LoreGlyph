<template>
  <header class="header">
    <div
      class="left-header"
      @click="$router.push('/languages')"
      title="Вернуться на главную страницу"
    >
      <img class="logo" src="../assets/cube_white_logo.svg" alt="logo" />
      <h1>LoreGlyph</h1>
    </div>
    <div class="right-header">
      <button @click="$router.push('/languages')" class="right-header-buttons">
        Вернуться
      </button>
      <button @click="logout" class="right-header-buttons">Выйти</button>
    </div>
  </header>
  <main>
    <h1 class="main-title">Мой аккаунт</h1>
    <h1 class="warning-developer">Профиль в разработке...</h1>

    <div class="sections">
      <div class="avatar-section">
        <div class="avatar-download-section">
          <img
            class="avatar-image"
            :src="avatarUrl"
            @error="onAvatarError"
            alt="avatar"
          />
          <input
            ref="avatarInput"
            type="file"
            accept=".jpg,.jpeg,.png,.webp"
            style="display: none"
            @change="uploadAvatar"
          />

          <button class="download-avatar-btn" @click="avatarInput.click()">
            Загрузить аватарку
          </button>
        </div>
        <h1 class="title">{{ userName }}</h1>
        <p class="information">Дата регистрации: {{ createdBy }}</p>
      </div>

      <div class="data-section">
        <h1 class="title">Информация</h1>
        <p class="information">Логин: {{ login }}</p>
      </div>

      <div class="password-section">
        <h1 class="title">Смена пароля</h1>
        <input v-model="oldPassword" placeholder="Введите старый пароль" />
        <input v-model="newPassword" placeholder="Введите новый пароль" />
        <button @click="changePassword" class="save-changes-button">
          Сменить пароль
        </button>
      </div>
    </div>

    <button @click="deleteAccount" class="warning-button">
      Удалить аккаунт
    </button>
  </main>
  <FooterComponent />
</template>

<script setup>
import { ref, onMounted, computed } from "vue";
import { userService } from "@/services/userService";
import { authService } from "@/services/authService";
import { useToast } from "vue-toastification";
import { fileUrl } from "@/utils/url";

import FooterComponent from "@/components/FooterComponent.vue";
import defaultAvatar from "@/assets/default-avatar.svg";

const toast = useToast();

const userName = ref("");
const login = ref("");
const createdBy = ref("");

const oldPassword = ref("");
const newPassword = ref("");

const avatarInput = ref(null);
const avatarPath = ref("");

const avatarUrl = computed(() => fileUrl(avatarPath.value) || defaultAvatar);

const onAvatarError = (event) => {
  event.target.src = defaultAvatar;
};

const uploadAvatar = async (event) => {
  const file = event.target.files[0];
  if (!file) {
    return;
  }

  const formData = new FormData();
  formData.append("avatar", file);

  try {
    await userService.uploadAvatar(formData);

    await loadUser();

    toast.success("Аватарка загружена");
  } catch (e) {
    const message = e.response?.data || "Ошибка загрузки аватарки";
    
    toast.error(message);
  }
};

const loadUser = async () => {
  const res = await userService.getMe();
  avatarPath.value = res.data.avatarPath;
};

const changePassword = async () => {
  try {
    await authService.resetPassword({
      Login: login.value,
      OldPassword: oldPassword.value,
      NewPassword: newPassword.value,
    });
    if (!oldPassword.value || !newPassword.value) {
      toast.error("Заполните все поля");
      return;
    }

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
  if (!confirm("Удалить профиль. Вы не сможете его восстановить")) {
    return;
  }
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
    avatarPath.value = data.avatarPath;
  } catch (e) {
    toast.error("Ошибка загрузки профиля:", e);
  }
});
</script>

<style scoped>
.warning-button {
  display: block;
  border: none;
  color: var(--red);
  font-size: 1.5rem;
  font-family: "Montserrat-Regular", sans-serif;
  text-align: center;
  margin: 1rem auto;
  align-items: center;
  justify-content: center;
  padding: 1rem;
  border-radius: 1rem;
  background: var(--white);
  border: 0.1rem solid var(--red);
  cursor: pointer;
  transition: all 0.3s ease;
  width: fit-content;
}

.main-title {
  padding-top: 5.5rem;
  font-size: 2.2rem;
  text-align: center;
}

.warning-developer {
  font-size: 1.2rem;
  color: var(--middle-gray);
  text-align: center;
  padding: 1rem;
  background: rgba(0, 0, 0, 0.05);
  border-radius: 0.5rem;
  margin: 1rem auto;
  max-width: 90%;
}

.sections {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 2rem;
  padding: 1rem;
  max-width: 800px;
  margin: 0 auto;
  width: 100%;
}

.avatar-section,
.data-section,
.password-section {
  background: var(--white);
  border-radius: 1.5rem;
  padding: 1.5rem;
  width: 100%;
  max-width: 500px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
  transition:
    transform 0.3s ease,
    box-shadow 0.3s ease;
}

.avatar-section:hover,
.data-section:hover,
.password-section:hover {
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12);
}

.avatar-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
}

.avatar-download-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
}

.avatar-image {
  width: 150px;
  height: 150px;
  border-radius: 50%;
  object-fit: cover;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  background: var(--middle-light-gray);
}

.download-avatar-btn {
  margin-top: 0.5rem;
  padding: 0.6rem 1.2rem;
  border: none;
  border-radius: 2rem;
  color: var(--white);
  background: var(--middle-gray);
  cursor: pointer;
  font-family: "Montserrat-Bold", sans-serif;
  font-size: 0.9rem;
  transition: all 0.3s ease;
}

.download-avatar-btn:hover {
  background: var(--black-gray);
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.download-avatar-btn:active {
  transform: translateY(0);
}

.title {
  font-size: 1.5rem;
  font-family: "Montserrat-Bold", sans-serif;
  color: var(--black-gray);
  margin: 0.5rem 0;
  text-align: center;
}

.information {
  font-size: 1rem;
  font-family: "Montserrat-Regular", sans-serif;
  color: var(--middle-dark-gray);
  margin: 0.5rem 0;
  line-height: 1.5;
}

.password-section {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.password-section p {
  font-size: 0.9rem;
  color: var(--middle-gray);
  margin: 0;
  text-align: center;
}

.password-section input:focus {
  outline: none;
}

.password-section input::placeholder {
  color: var(--middle-dark-gray);
  opacity: 0.7;
  font-size: 0.9rem;
}

.save-changes-button {
  margin-top: 1rem;
  padding: 0.8rem 1.5rem;
  border: none;
  border-radius: 2rem;
  background-color: var(--black-gray);
  color: var(--white);
  font-family: "Montserrat-Bold", sans-serif;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.3s ease;
  width: 100%;
}

.save-changes-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  opacity: 0.9;
}

.save-changes-button:active {
  transform: translateY(0);
}

input {
  font-family: "Montserrat-Regular", sans-serif;
  font-size: 1.4rem;
  margin: 0.8rem 0;
  padding: 1rem 1rem 1rem 0.5rem;
  display: block;
  width: 100%;
  border: none;
  border-bottom: 0.1px solid var(--middle-dark-gray);
  background: transparent;
  transition: border-color 0.3s ease;
}

input:focus {
  outline: none;
  border-bottom-color: var(--light-brown);
}

input::placeholder {
  color: var(--middle-dark-gray);
  opacity: 0.7;
}

@media (min-width: 768px) {
  .sections {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 1.5rem;
    padding: 1.5rem;
    width: 100%;
    max-width: 200rem;
    margin: 0 auto;
  }

  .main-title {
    padding-top: 10rem;
  }

  .sections {
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    align-items: stretch;
    justify-content: center;
    gap: 2rem;
    padding: 1rem;
    max-width: 1200px;
    margin: 0 auto;
    width: 100%;
  }

  .avatar-section,
  .data-section,
  .password-section {
    background: var(--white);
    border-radius: 1.5rem;
    padding: 1.5rem;
    flex: 1;
    min-width: 250px;
    display: flex;
    flex-direction: column;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
    transition:
      transform 0.3s ease,
      box-shadow 0.3s ease;
  }

  .avatar-section {
    align-items: center;
    justify-content: flex-start;
  }

  .data-section {
    justify-content: flex-start;
  }

  .password-section {
    justify-content: space-between;
  }

  .password-section {
    display: flex;
    flex-direction: column;
    flex: 1;
  }

  .save-changes-button {
    margin-top: auto;
    width: 100%;
  }
}
</style>
