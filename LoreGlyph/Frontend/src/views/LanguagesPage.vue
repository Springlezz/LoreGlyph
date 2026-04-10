<template>
  <header class="header">
    <div class="left-header" title="Вернуться на главную страницу">
      <img class="logo" src="../assets/cube_black_logo.svg" alt="logo" />
      <h1>LoreGlyph</h1>
    </div>
    <div class="right-header">
      <button @click="logout" class="right-header-buttons">Выйти</button>
      <p>|</p>
      <button @click="goToProfile" class="right-header-buttons">Профиль</button>
    </div>
  </header>
  <main>
    <CreateLanguageModal
      v-if="showCreate"
      @close="showCreate = false"
      @created="loadLanguages"
    />
    <div class="welcome">
      <div class="left-section">
        <h1 class="greetings-titles">Добро пожаловать, {{ userName }}</h1>
        <h1 class="big-title">Мои языки</h1>
        <h2 class="description-text">Выберите свои созданные языки</h2>
      </div>
      <div class="right-section">
        <button @click="showCreate = true" class="main-menu-button">
          Создать новый
        </button>
      </div>
    </div>

    <div class="bottom-section-items">
      <div
        class="item-languages"
        v-for="lang in languages"
        :key="lang.languageId"
      >
        <img class="picture" src="../assets/pictures/image-for-reading.png" />

        <div class="description">
          <div class="left-item">
            <h1 class="bold-text">
              {{ lang.name }}
            </h1>

            <h2 class="description-text">
              {{ lang.description }}
            </h2>
          </div>

          <div class="right-item">
            <button
              @click="goToLanguage(lang.languageId)"
              class="edit-language-button"
            >
              Редактировать
            </button>

            <button
              class="delete-language"
              @click="deleteLanguage(lang.languageId)"
            >
              Удалить
            </button>
          </div>
        </div>
        <img class="line" src="../assets/Line.svg" />
      </div>
    </div>
  </main>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import { languageService } from "@/services/languageService";
import CreateLanguageModal from "@/components/CreateLanguageModal.vue";
import { useToast } from "vue-toastification";

import FooterComponent from "@/components/FooterComponent.vue";

const toast = useToast();

const router = useRouter();

const userName = ref(localStorage.getItem("userName"));
const languages = ref([]);

const showCreate = ref(false);

const deleteLanguage = async (id) => {
  if (!confirm("Удалить язык?")) return;

  try {
    await languageService.delete(id);
    await loadLanguages();
  } catch (e) {
    toast.error("Ошибка удаления");
  }
};

const goToLanguage = (id) => {
  router.push(`/language/${id}`);
};

const loadLanguages = async () => {
  try {
    const res = await languageService.getAll();
    languages.value = res.data;
  } catch (e) {
    console.log(e);
  }
};

onMounted(loadLanguages);

const logout = () => {
  localStorage.clear();
  window.location.href = "/home";
};

const goToProfile = () => {
  router.push("/profile");
};
</script>

<style scoped>
main {
  padding: 1rem 1.6rem;
}

.big-title {
  font-family: "Montserrat-Bold", sans-serif;
  font-size: 2rem;
}

.greetings-titles {
  font-size: 1.5rem;
}

.bold-text {
  font-family: "Montserrat-Bold", sans-serif;
}

.right-header {
  display: flex;
  align-items: center;
  gap: 15px;
  padding: 0 2rem;
}

.description-text {
  font-size: 1.2rem;
  font-family: "Montserrat-Light", sans-serif;
}

.picture {
  height: auto;
  width: 100%;
  max-width: 20rem;
  border-radius: 1rem;
}

.welcome {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  gap: 1.5rem;
  padding-bottom: 2rem;
}

.description {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  gap: 1rem;
  padding: 1rem;
}

.right-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
}

.left-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  gap: 0.8rem;
}

.edit-language-button {
  font-family: "Montserrat-Bold", sans-serif;
  font-size: 1.3rem;
  border: none;
  background: var(--main-gradient);
  color: var(--white);
  padding: 1.5rem 1rem;
  border-radius: 2rem;
  cursor: pointer;
  transition: all 0.3s ease;
  width: 100%;
  max-width: 100%;
}

.edit-language-button:hover {
  transform: scale(1.05);
  transition: transform 0.4s ease-in-out;
}

.delete-language {
  font-family: "Montserrat-Bold", sans-serif;
  font-size: 1.3rem;
  border: none;
  background-color: var(--white);
  color: var(--red);
  padding: 1.5rem 1rem;
  border-radius: 2rem;
  cursor: pointer;
  box-shadow:
    rgb(105, 11, 11) 0px 0.0625em 0.0625em,
    rgba(255, 8, 8, 0.25) 0px 0.125em 0.5em,
    rgba(255, 255, 255, 0.1) 0px 0px 0px 1px inset;
  transition: all 0.3s ease;
  width: 100%;
  max-width: 100%;
}

.delete-language:hover {
  transform: scale(1.05);
  transition: transform 0.4s ease-in-out;
}

.bottom-section-items {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
  justify-content: center;
  flex-wrap: wrap;
}

.item-languages {
  width: 100%;
  text-align: center;
}

@media (min-width: 768px) {
  .welcome {
    flex-direction: row;
    justify-content: space-between;
    align-items: flex-start;
    text-align: left;
  }

  .description {
    flex-direction: row;
    justify-content: space-between;
    text-align: left;
  }

  .right-item {
    flex-direction: row;
    align-items: flex-start;
  }

  .right-header {
    display: flex;
    align-items: center;
    gap: 15px;
    padding: 0 2rem;
  }
  .left-item {
    align-items: flex-start;
    text-align: left;
  }

  .edit-language-button,
  .delete-language {
    width: auto;
    padding: 2rem 1rem;
  }

  .bottom-section-items {
    flex-direction: row;
    gap: 2rem;
  }
  main {
    padding: 5rem 7.5rem;
  }

  .bottom-section-items {
    gap: 4rem 20rem;
  }

  .item-languages {
    width: 70rem;
  }

  .edit-language-button:hover,
  .delete-language:hover {
    transform: scale(1.1);
  }
  .picture {
    height: auto;
    width: 100%;
    max-width: 50rem;
    border-radius: 3rem;
  }

  .greetings-titles {
    font-size: 2.5rem;
  }

  .item-languages {
    width: auto;
    max-width: 50rem;
  }
}
</style>
