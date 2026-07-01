<template>
  <header class="header">
    <div
      @click="$router.push('/home')"
      class="left-header"
      title="Вернуться на главную страницу"
    >
      <img class="logo" src="../assets/cube_white_logo.svg" alt="logo" />
      <h1>LoreGlyph</h1>
    </div>
    <div class="right-header">
      <button class="right-header-buttons" @click="$router.push('/home')">
        Главная страница
      </button>
    </div>
  </header>
  <ErrorComponent v-if="hasError" />

  <main v-else>
    <span class="panorama"></span>

    <h1 class="title-share">С вами поделились языком "{{ languageName }}"</h1>

    <div class="section-author-download">
      <div class="left-section-author-download">
        <div class="about-author">
          <h1>Автор: {{ username }}</h1>
          <img
            class="mini-avatar"
            :src="avatarUrl || '/src/assets/default-avatar.svg'"
          />
        </div>
      </div>

      <div class="center-section-author-download">
        <div class="search" id="search">
          <input
            v-model="filterQuery"
            class="filter"
            type="search"
            placeholder="Поиск по словам..."
          />
          <img class="loupe" src="../assets/loupe-search.svg" alt="search" />
        </div>
      </div>

      <div class="right-section-author-download">
        <div class="buttons-menu">
          <button @click="downloadTable" class="download-table">
            Скачать таблицу
          </button>
        </div>
      </div>
    </div>

    <span class="line"></span>

    <h2 class="nothing-warning" v-if="words.length === 0 && !filterQuery">
      Упс. Тут ничего нет :(
    </h2>
    <h2
      class="nothing-warning"
      v-if="filteredWords.length === 0 && filterQuery"
    >
      По запросу "{{ filterQuery }}" ничего не найдено
    </h2>

    <div class="bottom-section-items" ref="dragContainer">
      <div
        v-for="(word, index) in filteredWords"
        :key="word.wordId"
        :data-id="word.wordItem"
      >
        <div class="item-words">
          <div class="left-section">
            <template class="created-word" v-if="editingId !== word.wordId">
              <h3>{{ word.text }}</h3>
              <h3 class="transcription">[{{ word.transcription }}]</h3>
              <h3>{{ word.translation }}</h3>
            </template>
          </div>
        </div>
      </div>
    </div>
  </main>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { useRoute } from "vue-router";
import * as XLSX from "xlsx";
import { wordService } from "@/services/wordService";
import ErrorComponent from "@/components/ErrorComponent.vue";
import { fileUrl } from "@/utils/url";

const username = ref("");
const avatarUrl = ref(null);
const languageName = ref("");

const route = useRoute();
const token = route.params.token;

const words = ref([]);
const filterQuery = ref("");
const hasError = ref(false);
const isLoading = ref(true);

const filteredWords = computed(() => {
  if (!filterQuery.value) return words.value;

  const q = filterQuery.value.toLowerCase().trim();

  return words.value.filter(
    (w) =>
      w.text.toLowerCase().includes(q) ||
      w.transcription.toLowerCase().includes(q) ||
      w.translation.toLowerCase().includes(q),
  );
});

const loadSharedData = async () => {
  isLoading.value = true;
  hasError.value = false;

  try {
    const res = await wordService.getSharedWords(token);

    if (!res.data?.words) {
      hasError.value = true;
      return;
    }

    username.value = res.data.authorName;
    avatarUrl.value = fileUrl(res.data.authorAvatarUrl);
    languageName.value = res.data.languageName;
    words.value = res.data.words;
  } catch (e) {
    hasError.value = true;
  } finally {
    isLoading.value = false;
  }
};

const downloadTable = () => {
  const exportData = words.value.map((w, i) => ({
    "№": i + 1,
    Слово: w.text,
    Транскрипция: w.transcription,
    Перевод: w.translation,
  }));

  const ws = XLSX.utils.json_to_sheet(exportData);
  const wb = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(wb, ws, "Words");
  XLSX.writeFile(wb, "words.xlsx");
};

onMounted(() => {
  loadSharedData();
});
</script>

<style scoped>
.section-author-download {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
}

.panorama {
  margin-top: 6rem;
}

.item-words {
  gap: 1rem;
  padding: 1rem;
  border-radius: 1rem;
  box-shadow: rgba(0, 0, 0, 0.1) 0px 1px 3px;
  margin: 2rem 0;
}

.left-section {
  display: flex;
  flex-direction: row;
  gap: 1rem;
  font-family: "Montserrat-Light", sans-serif;
  align-items: flex-start;
  flex-wrap: wrap;
}

.transcription {
  color: var(--middle-gray);
  font-style: italic;
}

.download-table {
  padding: 0.5rem 1rem;
  border-radius: 1rem;
  background-color: var(--white);
  box-shadow:
    rgba(0, 0, 0, 0.02) 0px 1px 3px 0px,
    rgba(27, 31, 35, 0.15) 0px 0px 0px 1px;
  color: var(--black-gray);
  border: none;
  cursor: pointer;
  font-family: "Montserrat-Regular", sans-serif;
  font-size: 1.2rem;
  transition: all 0.3s ease;
}

.download-table:hover {
  color: var(--black);
  background: var(--light-gray);
  transform: scale(1.05);
}

.buttons-menu {
  display: flex;
  gap: 1rem;
  align-items: center;
  text-align: center;
  justify-content: center;
  padding: 1rem 0 1rem 0;
}

@media (min-width: 768px) {
  .section-author-download {
    flex-direction: row;
    justify-content: center;
    align-items: center;
    gap: 2rem;
  }

  .item-words {
    padding: 1.5rem;
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
    box-shadow:
      rgba(0, 0, 0, 0.02) 0px 1px 3px 0px,
      rgba(27, 31, 35, 0.15) 0px 0px 0px 1px;
  }

  .left-section {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100%;
    max-width: 200rem;
    margin: 0 auto;
    gap: 1.5rem;
  }

  .bottom-section-items {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 1.5rem;
    padding: 1.5rem 0;
    width: 100%;
    max-width: 200rem;
    margin: 0 auto;
  }

  .download-table {
    font-size: 1.1rem;
    padding: 1rem;
    border-radius: 2rem;
  }
}
</style>
