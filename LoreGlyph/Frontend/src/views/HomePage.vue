<template>
  <div class="modal-overlay">
    <header class="header">
      <div class="left-header">
        <img class="logo" src="../assets/cube_white_logo.svg" alt="logo" />
        <h1>LoreGlyph</h1>
      </div>
      <div class="right-header">
        <button
          class="right-header-buttons"
          @click="openRegister"
          href="/login"
        >
          Регистрация
        </button>
        <button class="right-header-buttons" @click="openLogin" href="/login">
          Войти
        </button>
      </div>
    </header>
    <div>
      <div class="main-card">
        <div class="main-image">
          <img src="../assets/BackgroundCastle.svg" alt="Замок" />
        </div>
        <div class="main-image-content">
          <h1 class="welcome-title">Слова, рожденные воображением</h1>
          <h3 class="description-title">
            Чтобы создать свой язык, вам нужно зарегистрироваться или войти в
            свой аккаунт
          </h3>
          <button @click="openRegister" class="main-menu-button">
            Зарегистрироваться
          </button>
          <button @click="openLogin" class="main-menu-button">Войти</button>
        </div>
      </div>
    </div>
    <main>
      <div>
        <RegisterModal v-if="showRegister" @close="showRegister = false" />
        <ResetPasswordModal v-if="showReset" @close="showReset = false" />
        <LoginModal
          v-if="showLogin"
          @close="showLogin = false"
          @open-register="openRegister"
          @open-reset="openReset"
        />
      </div>

      <div class="card-sections">
        <div class="item-section">
          <img class="icon-card" src="../assets/book.svg" />
          <h3 class="title-card">Создание языков без ограничений</h3>
          <p class="description-card">
            Создавайте языки, добавляйте слова, добавляйте описания
          </p>
        </div>

        <div class="item-section">
          <img class="icon-card" src="../assets/board.svg" />
          <h3 class="title-card">Скачивайте таблицу слов</h3>
          <p class="description-card">
            Удобно экспортируйте и сохраняйте словарь
          </p>
        </div>

        <div class="item-section">
          <img class="icon-card" src="../assets/people.svg" />
          <h3 class="title-card">Удобный инструмент</h3>
          <p class="description-card">
            Используйте для своих вселенных, ДнД партий и написания произведений
          </p>
        </div>
      </div>
      <h2 class="text-about-create">
        Здесь можно создать свой словарь слов, перевод и произношение для своих
        игр
      </h2>

      <div class="about-project-section">
        <h1 class="about-project">О проекте</h1>
        <p>
          Привет! Я написала этот сайт, потому что сама захотела удобный
          инструмент для друзей.<br />
          Сейчас это первая версия, но в планах - регулярные обновления: новые
          фичи, исправления и просто полезные мелочи.
        </p>
        <p>
          Мне очень приятно, если вы пользуетесь проектом. Это вдохновляет
          делать его лучше.
        </p>
        <p>Апдейты будут - обещаю!</p>
        <h3 class="about-project">Что я добавлю?</h3>
        <ul class="list">
          <li>Английскую локализацию</li>
          <li>Возможно создавать совместные языки, делиться ими</li>
          <li>Возможность авторизоваться через соцсети</li>
          <li>Возможность добавлять иероглифы, рисовать их</li>
        </ul>
      </div>
    </main>
    <FooterComponent />
  </div>
</template>

<script setup>
import { ref } from "vue";
import RegisterModal from "@/components/RegisterModal.vue";
import ResetPasswordModal from "@/components/ResetPasswordModal.vue";
import LoginModal from "@/components/LoginModal.vue";
import FooterComponent from "@/components/FooterComponent.vue";

const showRegister = ref(false);
const showReset = ref(false);
const showLogin = ref(false);

const openRegister = () => {
  showRegister.value = true;
  showLogin.value = false;
  showReset.value = false;
};

const openLogin = () => {
  showRegister.value = false;
  showLogin.value = true;
  showReset.value = false;
};

const openReset = () => {
  showRegister.value = false;
  showLogin.value = false;
  showReset.value = true;
};
</script>

<style scoped>
ul {
  padding-left: 2rem;
}

</style>
