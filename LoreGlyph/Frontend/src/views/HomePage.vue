<template>
  <div class="modal-overlay">
    <header class="header">
      <div class="left-header">
        <img class="logo" src="../assets/cube_black_logo.svg" alt="logo" />
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
        <p>|</p>
        <button class="right-header-buttons" @click="openLogin" href="/login">
          Войти
        </button>
      </div>
    </header>
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
      <div class="left-section">
        <div class="greetings">
          <h1 class="greetings-titles">Добро пожаловать!</h1>
          <h1 class="colored-black-text">
            Здесь можно создать свой словарь слов, перевод и произношение для
            <span class="colored-text-brown">своих игр</span>
          </h1>
        </div>

        <h1 class="attention-text">
          Чтобы создать свой язык, вам нужно зарегистрироваться или войти в
          аккаунт
        </h1>
        <button @click="openRegister" class="main-menu-button">
          Регистрация аккаунта
        </button>
        <button @click="openLogin" class="main-menu-button">
          Войти в аккаунт
        </button>
      </div>

      <div class="right-section">
        <img
          class="picture-with-elf"
          src="../assets/pictures/generated.png"
          alt="Изображение эльфов"
        />
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
p {
  margin: 0;
  color: var(--white);
}

main {
  margin-top: 2rem;
  padding: 0rem 2rem;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.greetings {
  font-size: 1.2rem;
  width: 100%;
  word-break: break-word;
  word-wrap: break-word;
  white-space: normal;
}

.colored-black-text {
  font-family: "Montserrat-Bold", sans-serif;
  color: var(--black);
}

.colored-text-brown {
  color: var(--light-brown);
  font-family: "Montserrat-Bold", sans-serif;
}

.attention-text {
  padding: 2rem 0rem;
  font-family: "Montserrat-Light", sans-serif;
  color: var(--black);
  font-size: 1.2rem;
  width: 100%;
  word-break: break-word;
  word-wrap: break-word;
  white-space: normal;
}

.picture-with-elf {
  height: auto;
  width: 100%;
  max-width: 100%;
  border-radius: 2rem;
  box-shadow: 0px 0px 10px 0px rgba(0, 0, 0, 0.5);
}

.main-menu-button:hover {
  transform: scale(1.1);
  transition: transform 0.4s ease-in-out;
}

@media (min-width: 1025px) {
  main {
    padding: 8rem 2.5rem;
    flex-direction: row;
  }

  .greetings {
    width: 46rem;
  }

  .attention-text {
    width: 36rem;
    font-size: 1.5rem;
  }

  .picture-with-elf {
    max-width: 60rem;
  }
}
</style>
