<template>
  <header class="header">
    <div class="left-header" title="Вернуться на главную страницу">
      <img class="logo" src="../assets/cube_black_logo.svg" alt="logo" />
      <h1>LoreGlyph</h1>
    </div>
    <div class="right-header">
      <button @click="logout" class="right-header-buttons">Выйти</button>
      <p>|</p>
      <button @click="goToProfile" class="right-header-buttons">
        Мой аккаунт
      </button>
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

            <button class="download-table">Скачать таблицу</button>
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
    alert("Ошибка удаления");
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
  padding: 5rem 7.5rem;
}

.big-title {
  font-family: "Montserrat-Bold";
  font-size: 2.5rem;
}

.greetings-titles {
  font-size: 2.5rem;
}
.bold-text {
  font-family: "Montserrat-Bold";
}

.description-text {
  font-size: 1.5rem;
  font-family: "Montserrat-Light";
}

.picture {
  height: auto;
  width: 100%;
  max-width: 50rem;
  border-radius: 3rem;
}

.welcome {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  padding-bottom: 2rem;
}

.description {
  display: flex;

  justify-content: space-between;
  padding: 1rem;
  text-align: left;
}

.edit-language-button {
  font-family: "Montserrat-Bold";
  font-size: 1.3rem;
  border: none;
  background: var(--main-gradient);
  color: var(--white);
  padding: 2rem 1rem;
  border-radius: 2rem;
  cursor: pointer;
}
.right-item {
  display: flex;
  flex-direction: row;
  gap: 1rem;
}
.left-item {
  display: flex;
  flex-direction: column;
  gap: 0.8rem;
}

.download-table {
  font-family: "Montserrat-Bold";
  font-size: 1.3rem;
  border: none;
  background-color: var(--white);
  padding: 2rem 1rem;
  border-radius: 2rem;
  box-shadow:
    rgba(0, 0, 0, 0.25) 0px 0.0625em 0.0625em,
    rgba(0, 0, 0, 0.25) 0px 0.125em 0.5em,
    rgba(255, 255, 255, 0.1) 0px 0px 0px 1px inset;
  cursor: pointer;
}

.bottom-section-items {
  display: flex;
  flex-direction: row;
  gap: 4rem 20rem;
  justify-content: center;
  flex-wrap: wrap;
}

.delete-language {
  font-family: "Montserrat-Bold";
  font-size: 1.3rem;
  border: none;
  background-color: var(--white);
  color: var(--red);
  padding: 2rem 1rem;
  border-radius: 2rem;
  cursor: pointer;
  box-shadow:
    rgb(105, 11, 11) 0px 0.0625em 0.0625em,
    rgba(255, 8, 8, 0.25) 0px 0.125em 0.5em,
    rgba(255, 255, 255, 0.1) 0px 0px 0px 1px inset;
}

.item-languages {
  width: 50rem;
  text-align: center;
}
</style>
