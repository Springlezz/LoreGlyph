<template>
  <header class="header">
    <div class="left-header" title="Вернуться на главную страницу">
      <img class="logo" src="../assets/cube_white_logo.svg" alt="logo" />
      <h1>LoreGlyph</h1>
    </div>
    <div class="right-header">
      <button @click="logout" class="right-header-buttons">Выйти</button>
      <button @click="goToProfile" class="right-header-buttons">Профиль</button>
    </div>
  </header>
  <main>
    <CreateLanguageModal
      v-if="showCreate"
      @close="showCreate = false"
      @created="loadLanguages"
    />
    <span class="panorama"></span>
    <div class="welcome">
      <div class="left-section">
        <h1 class="greetings-titles">Добро пожаловать, {{ userName }}</h1>
        <h1 class="big-title">Мои языки</h1>
        <h2 class="description-text">Выберите язык, чтобы начать</h2>
      </div>
      <div class="right-section">
        <div class="search">
          <input
            v-model="filterQuery"
            class="filter"
            type="search"
            placeholder="Поиск по языкам..."
          />
          <img class="loupe" src="../assets/loupe-search.svg" alt="search" />
        </div>

        <button @click="showCreate = true" class="main-menu-button">
          Создать новый
        </button>
      </div>
    </div>

    <span class="line"></span>
    <h2 class="nothing-warning" v-if="languages.length === 0 && !filterQuery">
      Упс. Тут ничего нет :(<br />Нажмите "Создать новый", чтобы создать язык
    </h2>
    <div
      v-if="filteredLanguages.length === 0 && filterQuery"
      class="nothing-warning"
    >
      <h3>По запросу "{{ filterQuery }}" ничего не найдено</h3>
    </div>

    <div class="bottom-section-items">
      <div
        class="item-languages"
        v-for="lang in filteredLanguages"
        :key="lang.languageId"
      >
        <img
          @click="goToLanguage(lang.languageId)"
          class="picture"
          src="../assets/pictures/image-for-reading.png"
        />
        <div class="left-item">
          <h1 class="bold-text">
            {{ lang.name }}
          </h1>
          <h2 class="description-text">
            {{ lang.description }}
          </h2>
        </div>
        <div class="buttons-dlt-edt">
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
        <span class="line"></span>
      </div>
    </div>
  </main>
  <FooterComponent />
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import { languageService } from "@/services/languageService";
import CreateLanguageModal from "@/components/CreateLanguageModal.vue";
import { useToast } from "vue-toastification";
import { computed } from "vue";

import FooterComponent from "@/components/FooterComponent.vue";

const toast = useToast();

const router = useRouter();

const userName = ref(localStorage.getItem("userName"));
const languages = ref([]);

const showCreate = ref(false);

const filterQuery = ref("");

const filteredLanguages = computed(() => {
  return languages.value.filter((language) =>
    language.name.toLowerCase().includes(filterQuery.value.toLowerCase()),
  );
});

const deleteLanguage = async (id) => {
  if (!confirm("Удалить язык?")) {
    return;
  }

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
.right-section {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  align-items: center;
  text-align: center;
  justify-content: center;
  padding: 1rem 0 1rem 0;
}
.buttons-dlt-edt {
  display: flex;
  gap: 1rem;
  align-items: center;
  text-align: center;
  justify-content: center;
  padding: 1rem 0 1rem 0;
}

.delete-language {
  background: var(--white);
  color: var(--red);
  padding: 0.5rem 1rem;
  border-radius: 0.5rem;
  cursor: pointer;
  font-size: 0.9rem;
  font-family: "Montserrat-Bold", sans-serif;
  border: none;
  box-shadow:
    rgb(105, 11, 11) 0px 0.0625em 0.0625em,
    rgba(255, 8, 8, 0.25) 0px 0.125em 0.5em,
    rgba(255, 255, 255, 0.1) 0px 0px 0px 1px inset;
  transition: all 0.3s ease;
}

.delete-language:hover {
  background: var(--red);
  color: var(--white);
}

.edit-language-button {
  background: var(--black-gray);
  color: var(--white);
  padding: 0.5rem 1rem;
  border-radius: 0.5rem;
  cursor: pointer;
  font-size: 0.9rem;
  font-family: "Montserrat-Bold", sans-serif;
  border: none;
  transition: all 0.3s ease;
}

.picture {
  height: auto;
  width: 100%;
  max-width: 20rem;
  border-radius: 1rem;
  cursor: pointer;
}

.bottom-section-items {
  display: grid;
  grid-template-columns: 1fr;
  gap: 1.5rem;
  padding: 1rem;
  width: 100%;
  max-width: 1200px;
  margin: 0 auto;
}

.item-languages {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  width: 100%;
  background: var(--white);
  border-radius: 1rem;
  padding: 1.5rem;
  transition:
    transform 0.3s ease,
    box-shadow 0.3s ease;
}

.description {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: space-between;
  width: 100%;
  max-width: 100%;
  margin: 0 auto;
}

.left-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  gap: 0.8rem;
}

.main-menu-button {
  font-size: 1rem;
  padding: 1rem;
  width: 100%;
}

.big-title {
  font-family: "Baskerville-Regular", sans-serif;
  font-size: 2.5rem;
}

.greetings-titles {
  font-size: 1.5rem;
  color: var(--middle-gray);
  font-family: "Montserrat-ExtraLight", sans-serif;
}

.bold-text {
  font-family: "Montserrat-Bold", sans-serif;
}

.description-text {
  font-size: 1.2rem;
  font-family: "Baskerville-Regular", sans-serif;
}

.welcome {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  gap: 1.5rem;
  padding: 2rem 0 1rem 0;
}

@media (min-width: 768px) {
  .right-section {
    flex-direction: row;
  }
  .picture {
    height: auto;
    width: 100%;
    max-width: 30rem;
    border-radius: 1rem;
    cursor: pointer;
  }

  .bottom-section-items {
    grid-template-columns: repeat(2, 1fr);
    gap: 0rem;
    padding: 1.5rem;
  }
  .main-menu-button {
    font-size: 1.5rem;
  }

  .welcome {
    flex-direction: row;
    justify-content: space-between;
    align-items: flex-start;
    text-align: left;
  }

  .edit-language-button,
  .delete-language {
    width: auto;
    padding: 2rem 1rem;
    font-size: 1.2rem;
  }

  main {
    padding: 5rem 7.5rem;
  }
}
</style>
