<template>
  <header class="header">
    <div
      @click="$router.push('/languages')"
      class="left-header"
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
    <span class="panorama"></span>
    <h1 class="main-title">Редактирование языка {{ languageName }}</h1>

    <div class="buttons-menu">
      <button @click="downloadTable" class="download-table">
        Скачать таблицу
      </button>
      <button @click="addWord" class="add-word">Добавить слово</button>

      <button @click="toggleShare" class="share-btn">
        <img class="share-icon" src="../assets/share.svg" alt="share" />

        {{ shareInfo?.isPublic ? "Закрыть доступ" : "Поделиться" }}
      </button>
      <div
        @click="copyToClipboard"
        v-if="shareInfo?.isPublic"
        class="share-link"
      >
        <p>Скопировать ссылку</p>
        <img class="share-icon" src="../assets/copy.svg" alt="copy" />
      </div>
    </div>

    <div class="search" id="search">
      <input
        v-model="filterQuery"
        class="filter"
        type="search"
        placeholder="Поиск по словам..."
      />
      <img class="loupe" src="../assets/loupe-search.svg" alt="search" />
    </div>
    <span class="line"></span>

    <h2 class="nothing-warning" v-if="words.length === 0 && !filterQuery">
      Упс. Тут ничего нет :(<br />Нажмите "Добавить слово", чтобы создать слово
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
            <img class="burger" src="../assets/burger.svg" />

            <template class="created-word" v-if="editingId !== word.wordId">
              <h3>{{ word.text }}</h3>
              <h3 class="transcription">[{{ word.transcription }}]</h3>
              <h3>{{ word.translation }}</h3>
            </template>

            <template v-else>
              <div class="inputs">
                <input
                  class="input-word-translate-transcription"
                  v-model="editForm.text"
                  placeholder="Слово"
                />
                <input
                  class="input-word-translate-transcription"
                  v-model="editForm.transcription"
                  placeholder="Транскрипция"
                />
                <input
                  class="input-word-translate-transcription"
                  v-model="editForm.translation"
                  placeholder="Перевод"
                />
              </div>
            </template>
          </div>

          <div class="right-section">
            <button class="edit-word-button" @click="toggleEdit(word)">
              {{ editingId === word.wordId ? "Сохранить" : "Редактировать" }}
            </button>

            <button class="delete-word-button" @click="deleteWord(word.wordId)">
              Удалить
            </button>
          </div>
        </div>
      </div>
    </div>
  </main>
</template>

<script setup>
import { ref, onMounted, onUnmounted, computed } from "vue";
import { useRoute } from "vue-router";
import * as XLSX from "xlsx";
import Sortable from "sortablejs";
import { wordService } from "@/services/wordService";
import FooterComponent from "@/components/FooterComponent.vue";

import { languageService } from "@/services/languageService";

import { useToast } from "vue-toastification";

const toast = useToast();

const route = useRoute();
const languageId = ref(route.params.id);
const languageName = ref("");

const words = ref([]);
const editingId = ref(null);
const dragContainer = ref(null);

const shareInfo = ref(null);

let sortable = null;

const editForm = ref({
  text: "",
  transcription: "",
  translation: "",
});

const STORAGE_KEY = `words_${languageId.value}`;

const filterQuery = ref("");

const filteredWords = computed(() => {
  if (!filterQuery.value) {
    return words.value;
  }

  const comp = filterQuery.value.toLowerCase().trim();

  return words.value.filter((word) => {
    return (
      word.text.toLowerCase().includes(comp) ||
      word.transcription.toLowerCase().includes(comp) ||
      word.translation.toLowerCase().includes(comp)
    );
  });
});

const copyToClipboard = () => {
  navigator.clipboard.writeText(shareLink.value);
  toast.success("Ссылка скопирована");
};

const toggleShare = async () => {
  try {
    if (shareInfo.value?.isPublic) {
      await languageService.unshare(languageId.value);

      shareInfo.value.isPublic = false;

      toast.success("Доступ закрыт");
    } else {
      const res = await languageService.share(languageId.value);

      shareInfo.value = {
        isPublic: true,
        shareToken: res.data,
      };

      toast.success("Доступ открыт");
    }
  } catch {
    toast.error("Ошибка изменения доступа");
  }
};

const loadShareInfo = async () => {
  try {
    const res = await languageService.getShareInfo(languageId.value);

    shareInfo.value = res.data;
  } catch {
    toast.error("Не удалось получить информацию об открытом доступе");
  }
};

const saveToLocalStorage = () => {
  if (!words.value.length && localStorage.getItem(STORAGE_KEY)) {
    return;
  }
  localStorage.setItem(STORAGE_KEY, JSON.stringify(words.value));
};

const shareLink = computed(() => {
  return `${window.location.origin}/shared-language/${shareInfo.value.shareToken}`;
});

const loadLanguage = async () => {
  try {
    const res = await languageService.getAll();

    const language = res.data.find((l) => l.languageId === languageId.value);

    if (language) {
      languageName.value = language.name;
    }
  } catch (e) {
    toast.error("Ошибка загрузки языка");
  }
};

const loadWords = async () => {
  const savedWordsJSON = localStorage.getItem(STORAGE_KEY);
  const savedWords = savedWordsJSON ? JSON.parse(savedWordsJSON) : [];

  try {
    const res = await wordService.getAll(languageId.value);
    const serverWords = res.data.sort((a, b) => a.order - b.order);
    const merged = [...serverWords];

    savedWords.forEach((w) => {
      if (w.isOffline && !merged.find((sw) => sw.wordId === w.wordId)) {
        merged.push(w);
      }
    });

    if (merged.length === 0 && savedWords.length > 0) {
      words.value = savedWords;
    } else {
      words.value = merged;
    }

    if (words.value.length > 0 || !localStorage.getItem(STORAGE_KEY)) {
      saveToLocalStorage();
    }
  } catch (e) {
    toast.error("Ошибка загрузки с сервера");
    words.value = savedWords;
    if (words.value.length > 0) {
      saveToLocalStorage();
    }
  }
};

const addWord = async () => {
  try {
    const newWord = {
      text: "",
      transcription: "",
      translation: "",
      order: words.value.length,
      languageId: languageId.value,
    };
    const res = await wordService.create(languageId.value, newWord);

    words.value.push(res.data);
    saveToLocalStorage();
    startEdit(res.data);
  } catch (e) {
    toast.error("Ошибка добавления слова");
    const tempId = Date.now();
    const tempWord = {
      wordId: tempId,
      languageId: languageId.value,
      text: "",
      transcription: "",
      translation: "",
      order: words.value.length,
      isOffline: true,
    };
    words.value.push(tempWord);
    saveToLocalStorage();
    startEdit(tempWord);
  }
};

const startEdit = (word) => {
  editForm.value = {
    text: word.text,
    transcription: word.transcription,
    translation: word.translation,
  };
  editingId.value = word.wordId;
};

const saveEdit = async (wordId) => {
  try {
    const word = words.value.find((w) => w.wordId === wordId);
    if (!word) {
      return;
    }

    if (
      !editForm.value.text ||
      !editForm.value.transcription ||
      !editForm.value.translation
    ) {
      toast.error("Заполните все поля");
      return;
    }
    await wordService.update(wordId, {
      text: editForm.value.text,
      transcription: editForm.value.transcription,
      translation: editForm.value.translation,
    });

    const index = words.value.findIndex((w) => w.wordId === wordId);
    if (index !== -1) {
      words.value[index] = {
        ...words.value[index],
        text: editForm.value.text,
        transcription: editForm.value.transcription,
        translation: editForm.value.translation,
      };
      saveToLocalStorage();
    }

    editingId.value = null;
    editForm.value = { text: "", transcription: "", translation: "" };
  } catch (e) {
    toast.error("Ошибка сохранения слова");
    const index = words.value.findIndex((w) => w.wordId === wordId);
    if (index !== -1) {
      words.value[index] = {
        ...words.value[index],
        text: editForm.value.text,
        transcription: editForm.value.transcription,
        translation: editForm.value.translation,
      };
      saveToLocalStorage();
    }
    editingId.value = null;
    editForm.value = { text: "", transcription: "", translation: "" };
  }
};

const cancelEdit = () => {
  editingId.value = null;
  editForm.value = { text: "", transcription: "", translation: "" };
};

const toggleEdit = (word) => {
  if (editingId.value === word.wordId) {
    saveEdit(word.wordId);
  } else {
    startEdit(word);
  }
};

const deleteWord = async (wordId) => {
  if (!confirm("Удалить слово?")) {
    return;
  }

  try {
    await wordService.delete(wordId);
    words.value = words.value.filter((w) => w.wordId !== wordId);
    saveToLocalStorage();

    if (editingId.value === wordId) {
      cancelEdit();
    }
  } catch (e) {
    toast.error("Ошибка удаления:", e);
    words.value = words.value.filter((w) => w.wordId !== wordId);
    saveToLocalStorage();

    if (editingId.value === wordId) {
      cancelEdit();
    }
  }
};

onMounted(() => {
  languageId.value = route.params.id;

  if (!languageId.value) {
    toast.error("ID языка не указан");
    return;
  }
  loadLanguage();
  loadWords();
  loadShareInfo();

  if (dragContainer.value) {
    sortable = new Sortable(dragContainer.value, {
      animation: 200,
      onEnd: async (event) => {
        const { oldIndex, newIndex } = event;
        if (oldIndex !== newIndex) {
          const movedItem = words.value.splice(oldIndex, 1)[0];
          words.value.splice(newIndex, 0, movedItem);

          const updatedOrder = words.value.map((w, idx) => ({
            wordId: w.wordId,
            order: idx,
          }));

          saveToLocalStorage();

          try {
            await wordService.updateOrder(languageId.value, updatedOrder);
          } catch (e) {
            toast.error("Ошибка сохранения порядка");
          }
        }
      },
    });
  }
});
const downloadTable = () => {
  const sortedWords = [...words.value].sort((a, b) => a.order - b.order);
  const exportData = sortedWords.map((word, index) => ({
    "№": index + 1,
    Слово: word.text,
    Транскрипция: word.transcription,
    Перевод: word.translation,
  }));

  const worksheet = XLSX.utils.json_to_sheet(exportData);

  const workbook = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(workbook, worksheet, "Words");
  XLSX.writeFile(workbook, "words.xlsx");
};
const logout = () => {
  localStorage.clear();
  window.location.href = "/home";
};

onUnmounted(() => {
  if (sortable) {
    sortable.destroy();
  }
});
</script>

<style scoped>
.share {
  display: block;
  width: 100%;
}

.share-link {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  text-align: center;
  gap: 0.75rem;
  width: 100%;
  cursor: pointer;
}

.share-link:hover {
  transform: scale(1.05);
  transition: all 0.3s ease;
}

.share-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  width: 100%;
  padding: 0.75rem 1rem;
  border-radius: 1rem;
  background-color: var(--white);
  box-shadow:
    rgba(0, 0, 0, 0.02) 0px 1px 3px 0px,
    rgba(27, 31, 35, 0.15) 0px 0px 0px 1px;
  color: var(--black-gray);
  border: none;
  cursor: pointer;
  font-family: "Montserrat-Regular", sans-serif;
  transition: all 0.3s ease;
}

.share-btn:hover {
  color: var(--black);
  background: var(--light-gray);
  transform: scale(1.05);
}

.share-icon {
  width: 1.2rem;
  flex-shrink: 0;
}

.main-title {
  padding-top: 1rem;
}

.panorama {
  margin-top: 6rem;
}

.burger {
  width: 1.5rem;
}

.item-words {
  gap: 1rem;
  padding: 1rem;
  border-radius: 1rem;
  box-shadow: rgba(0, 0, 0, 0.1) 0px 1px 3px;
  margin-top: 2rem;
}

.left-section {
  display: flex;
  flex-direction: row;
  gap: 1rem;
  font-family: "Montserrat-Light", sans-serif;
  align-items: flex-start;
  flex-wrap: wrap;
}

.left-section h3,
.left-section .transcription {
  margin: 0;
  word-wrap: break-word;
  word-break: break-word;
  white-space: normal;
  max-width: 100%;
}

.left-section .transcription {
  color: var(--middle-gray);
  font-style: italic;
}

.edit-word-button {
  padding: 0.3rem;
  border-radius: 0.5rem;
  color: var(--white);
  background: var(--black-gray);
  border: none;
  cursor: pointer;
  transition: all 0.3s ease;
  font-family: "Montserrat-Bold", sans-serif;
}

.edit-word-button:hover {
  background: var(--white);
  color: var(--black-gray);
  box-shadow:
    rgba(19, 17, 17, 0.74) 0px 1px 3px 0px,
    rgba(15, 14, 14, 0.911) 0px 0px 0px 1px;
  transform: scale(1.05);
}

.right-section {
  font-size: 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  margin-top: 1rem;
}

.delete-word-button {
  padding: 0.3rem;
  border-radius: 0.5rem;
  color: var(--red);
  background: var(--white);
  box-shadow:
    rgba(185, 33, 33, 0.74) 0px 1px 3px 0px,
    rgba(160, 55, 55, 0.911) 0px 0px 0px 1px;
  border: none;
  cursor: pointer;
  font-family: "Montserrat-Bold", sans-serif;
  transition: all 0.3s ease;
}

.delete-word-button:hover {
  background: var(--red);
  color: var(--white);
  transform: scale(1.05);
}

.inputs {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.add-word {
  padding: 0.5rem 1rem;
  border-radius: 1rem;
  color: var(--white);
  background: var(--black-gray);
  border: none;
  cursor: pointer;
  font-family: "Montserrat-Regular", sans-serif;
  transition: all 0.3s ease;
}

.add-word:hover {
  color: var(--white);
  background: var(--middle-gray);
  transform: scale(1.05);
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
  transition: all 0.3s ease;
}

.download-table:hover {
  color: var(--black);
  background: var(--light-gray);
  transform: scale(1.05);
}

.buttons-menu {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  margin: 1.5rem 0;
}

.buttons-menu button {
  width: 100%;
}

.share-link {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 0.5rem;
}

.input-word-translate-transcription {
  font-family: "Montserrat-Regular", sans-serif;
  font-size: 1rem;
  padding: 1rem;
  display: block;
  width: 100%;
  border: none;
  color: var(--middle-dark-gray);
  border-bottom: 0.1px solid var(--middle-dark-gray);
  word-break: break-word;
}

.input-word-translate-transcription:focus {
  outline: none;
  border-bottom: 0.1px solid var(--green);
}

@media (min-width: 768px) {
  .buttons-menu {
    display: flex;
    flex-direction: row;
    align-items: center;
    gap: 1rem;
  }

  .buttons-menu button {
    width: auto;
  }

  .share-link {
    width: auto;
    margin-left: 1rem;
  }

  .share {
    display: inline-block;
  }

  .share-btn {
    display: flex;
    align-items: center;
    gap: 0.5rem;
    font-size: 1.1rem;
    padding: 1rem;
    border-radius: 2rem;
    background-color: var(--white);
    box-shadow:
      rgba(0, 0, 0, 0.02) 0px 1px 3px 0px,
      rgba(27, 31, 35, 0.15) 0px 0px 0px 1px;
    color: var(--black-gray);
    border: none;
    cursor: pointer;
    font-family: "Montserrat-Regular", sans-serif;
    transition: all 0.3s ease;
  }

  .share-btn:hover {
    color: var(--white);
    background: var(--middle-gray);
    transform: scale(1.05);
  }

  .share-icon {
    width: 1.2rem;
  }

  .main-title {
    padding-top: 3rem;
    font-size: 2.5rem;
  }

  .burger {
    width: 2.4rem;
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
    padding: 1.5rem;
    width: 100%;
    max-width: 200rem;
    margin: 0 auto;
  }

  .add-word {
    font-size: 1.1rem;
    padding: 1rem;
    border-radius: 2rem;
  }

  .download-table {
    font-size: 1.1rem;
    padding: 1rem;
    border-radius: 2rem;
  }
}
</style>
