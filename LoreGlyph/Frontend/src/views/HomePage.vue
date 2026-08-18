<template>
  <div class="modal-overlay">
    <header class="header">
      <div class="left-header">
        <img class="logo" src="../assets/cube_white_logo.svg" alt="logo" />
        <h1>LoreGlyph</h1>
      </div>

      <button
        class="burger"
        :class="{ 'is-open': menuOpen }"
        @click="menuOpen = !menuOpen"
      >
        <img class="burger-line" src="../assets/burger.svg" alt="burger" />
      </button>

      <nav class="mobile-menu" :class="{ 'is-open': menuOpen }">
        <div class="center-header">
          <ul>
            <li
              class="right-header-buttons"
              @click="
                scrollTo('about');
                menuOpen = false;
              "
            >
              О проекте
            </li>
            <li
              class="right-header-buttons"
              @click="
                scrollTo('screenshots');
                menuOpen = false;
              "
            >
              Скриншоты
            </li>
            <li
              class="right-header-buttons"
              @click="
                scrollTo('faq');
                menuOpen = false;
              "
            >
              FAQ
            </li>
          </ul>
        </div>

        <div class="right-header">
          <button
            class="right-header-buttons"
            @click="
              openLogin();
              menuOpen = false;
            "
          >
            Войти
          </button>
          <button
            class="registration-header-button"
            @click="
              openRegister();
              menuOpen = false;
            "
          >
            Регистрация
          </button>
        </div>
      </nav>
    </header>
    <div>
      <div class="main-card">
        <div class="main-image">
          <img src="../assets/BackgroundCastle.webp" alt="Замок" />
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

      <h1 class="title-section">Возможности LoreGlyph</h1>

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

        <div class="item-section">
          <img class="icon-card" src="../assets/share-line.svg" />
          <h3 class="title-card">Делитесь с другими</h3>
          <p class="description-card">
            Создайте ссылку и делитесь своими словами с другими
          </p>
        </div>
      </div>

      <h1 class="title-section">О проекте</h1>
      <h2 class="text-about-create">Текст в разработке</h2>
      <h1 id="screenshots" class="title-section">Скриншоты</h1>
      <h2 id="about" class="text-about-create">Текст в разработке</h2>

      <h1 id="faq" class="title-section">FAQ</h1>

      <h2 class="text-about-create">
        Здесь можно создать свой словарь слов, перевод и произношение для своих
        игр
      </h2>
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
const menuOpen = ref(false);

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

const scrollTo = (id) => {
  document.getElementById(id)?.scrollIntoView({ behavior: "smooth" });
};
</script>

<style scoped>
.header {
  flex-wrap: nowrap;
}

.burger {
  display: flex;
  flex-direction: column;
  justify-content: center;
  gap: 0.3rem;
  width: 2.25rem;
  height: 2.25rem;
  padding: 0.5rem;
  background: none;
  border: none;
  cursor: pointer;
}

.center-header,
.right-header {
  display: none;
}

.mobile-menu {
  position: absolute;
  top: 100%;
  left: 0;
  right: 0;
  display: none;
  flex-direction: column;
  align-items: center;
  gap: 0.75rem;
  padding: 1rem;
  background: linear-gradient(
    180deg,
    rgba(255, 255, 255, 2) 10%,
    rgba(255, 255, 255, 0.9) 100%
  );
  backdrop-filter: blur(10px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
  border-radius: 0 0 1rem 1rem;
}

.mobile-menu.is-open {
  display: flex;
}

.mobile-menu .center-header {
  display: flex;
}

.mobile-menu .center-header ul {
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;
}

.mobile-menu .center-header li {
  margin: 0.3rem 0;
}

.mobile-menu .right-header {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.75rem;
}

.left-header {
  gap: 0.3rem;
}

.logo {
  max-width: 1.875rem;
}

.left-header h1 {
  font-size: 0.9rem;
}

.main-card {
  margin-top: 0;
}

.main-image {
  height: 18rem;
}

.main-image-content {
  padding: 1.125rem;
}

.description-title {
  margin: 0.75rem 0;
  font-size: 0.6rem;
}

.main-menu-button {
  margin-top: 0.375rem;
  font-size: 0.675rem;
  padding: 0.375rem;
}

.icon-card {
  max-width: 3rem;
}

.title-card {
  font-size: 0.75rem;
}

.description-card {
  font-size: 0.6rem;
}

.text-about-create {
  margin-top: 0.75rem;
  font-size: 0.9rem;
}

.about-project-section {
  font-size: 0.825rem;
}

.about-project {
  padding: 0.75rem 0;
  font-size: 1.35rem;
}

@media (min-width: 768px) {
  .header {
    flex-wrap: wrap;
  }

  .burger {
    display: none;
  }

  .mobile-menu {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    height: 100%;
    display: flex;
    flex-direction: row;
    justify-content: center;
    align-items: center;
    gap: 1rem;
    padding: 0;
    background: none;
    backdrop-filter: none;
    box-shadow: none;
  }

  .mobile-menu .center-header {
    display: flex;
  }

  .mobile-menu .center-header ul {
    flex-direction: row;
  }

  .mobile-menu .center-header li {
    margin: 0 0.8rem;
  }

  .mobile-menu .right-header {
    display: flex;
    flex-direction: row;
    align-items: center;
    gap: 0;
    position: absolute;
    right: 4.5rem;
  }

  .about-project {
    font-size: 2.25rem;
  }

  .about-project-section p,
  li {
    font-size: 1.125rem;
    margin: 0 6rem;
  }

  .text-about-create {
    font-size: 1.5rem;
    padding: 1.5rem;
  }

  .icon-card {
    max-width: 6rem;
  }

  .title-card {
    font-size: 1.5rem;
  }

  .description-card {
    margin-top: 0.7rem;
    font-size: 1.2rem;
  }

  .main-image {
    height: 36rem;
  }

  .main-image-content {
    padding: 0 4.5rem;
    width: 35%;
  }

  .description-title {
    font-size: 1.125rem;
  }

  .main-menu-button {
    font-size: 1.125rem;
    padding: 0.75rem;
  }

  .left-header h1 {
    font-size: 1.5rem;
  }

  .left-header {
    gap: 0.9rem;
  }

  .logo {
    max-width: 2.7rem;
  }

  .header {
    padding: 0 4.5rem;
    height: 4.5rem;
  }

  .right-header-buttons {
    font-size: 1.275rem;
  }
}
</style>
