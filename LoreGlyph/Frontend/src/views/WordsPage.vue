<template>
  <header class="header">
    <div
      @click="$router.push('/languages')"
      class="left-header"
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
    <h1 class="main-title">Язык</h1>
    <div class="buttons-menu">
      <button @click="downloadTable" class="download-table">
        Скачать таблицу
      </button>
      <button @click="addWord" class="add-word">Добавить слово</button>
    </div>
    <input class="filter" type="text" placeholder="Поиск по словам..." />

    <div class="bottom-section-items" ref="dragContainer">
      <div
        v-for="(word, index) in words"
        :key="word.wordId"
        :data-id="word.wordItem"
      >
        <div class="item-words">
          <div class="left-section">
            <img class="picture" src="../assets/burger.svg" />

            <template v-if="editingId !== word.wordId">
              <h3>{{ word.text }}</h3>
              <h3 class="transcription">{{ word.transcription }}</h3>
              <h3>{{ word.translation }}</h3>
            </template>

            <template v-else>
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
import { ref, onMounted, onUnmounted } from "vue";
import { useRoute } from "vue-router";
import * as XLSX from "xlsx";
import Sortable from "sortablejs";
import { wordService } from "@/services/wordService";

const route = useRoute();
const languageId = ref(route.params.id);
const words = ref([]);
const editingId = ref(null);
const dragContainer = ref(null);

let sortable = null;

const editForm = ref({
  text: "",
  transcription: "",
  translation: "",
});

const STORAGE_KEY = `words_${languageId.value}`;

const saveToLocalStorage = () => {
  if (!words.value.length && localStorage.getItem(STORAGE_KEY)) {
    return;
  }
  localStorage.setItem(STORAGE_KEY, JSON.stringify(words.value));
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
    alert("Ошибка загрузки с сервера");
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
    alert("Ошибка добавления слова");
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
    if (!word) return;

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
    alert("Ошибка сохранения слова");
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
  if (!confirm("Удалить слово?")) return;

  try {
    await wordService.delete(wordId);
    words.value = words.value.filter((w) => w.wordId !== wordId);
    saveToLocalStorage();

    if (editingId.value === wordId) {
      cancelEdit();
    }
  } catch (e) {
    console.error("Ошибка удаления:", e);
    words.value = words.value.filter((w) => w.wordId !== wordId);
    saveToLocalStorage();

    if (editingId.value === wordId) {
      cancelEdit();
    }
  }
};

onMounted(() => {
  languageId.value = route.params.id;
  if (languageId.value && typeof languageId.value === "string") {
    languageId.value = parseInt(languageId.value);
  }

  loadWords();

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
            alert("Ошибка сохранения порядка");
          }
        }
      },
    });
  }
});

const downloadTable = () => {
  const sortedWords = [...words.value].sort((a, b) => a.order - b.order);
  const exportData = sortedWords.map((word, index) => ({
    '№': index + 1,
    "#": word.order,
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
.input-word-translate-transcription {
  padding: 0.6rem;
  border-radius: 1rem;
  border: none;
  cursor: pointer;
  font-family: "Montserrat-Bold", sans-serif;
  box-shadow:
    rgba(0, 0, 0, 0.02) 0px 1px 3px 0px,
    rgba(27, 31, 35, 0.15) 0px 0px 0px 1px;
  transition: all 0.3s ease;
}

.input-word-translate-transcription:focus {
  outline: none;
}

.edit-word-button {
  background: var(--main-gradient);
  padding: 0.6rem;
  border-radius: 1rem;
  color: var(--white);
  border: none;
  cursor: pointer;
  font-family: "Montserrat-Bold", sans-serif;
  transition: all 0.3s ease;
}

.edit-word-button:hover {
  transform: scale(1.1);
}

.delete-word-button {
  background: var(--white);
  padding: 0.6rem;
  border-radius: 1rem;
  color: var(--red);
  border: none;
  cursor: pointer;
  font-family: "Montserrat-Bold", sans-serif;
  box-shadow:
    rgba(94, 94, 94, 0.25) 0px 0.0625em 0.0625em,
    rgba(0, 0, 0, 0.25) 0px 0.125em 0.5em,
    rgba(255, 255, 255, 0.1) 0px 0px 0px 1px inset;
  transition: all 0.3s ease;
}

.delete-word-button:hover {
  background: var(--red);
  color: var(--white);
  transform: scale(1.1);
  transition: transform 0.4s ease-in-out;
}

main {
  padding: 5rem 7.5rem;
}

.main-title {
  color: var(--black);
  font-family: "Montserrat-Regular", sans-serif;
  font-size: 2.5rem;
  margin-bottom: 2rem;
}

.buttons-menu {
  display: flex;
  justify-content: space-between;
  margin-bottom: 2rem;
}

.transcription {
  color: var(--gray-blue);

  white-space: nowrap;
}

.download-table {
  border: none;
  width: 20%;
  font-family: "Montserrat-Bold", sans-serif;
  background: none;
  font-size: 1.5rem;
  padding: 1rem 1rem;
  border-radius: 2rem;
  box-shadow:
    rgba(94, 94, 94, 0.25) 0px 0.0625em 0.0625em,
    rgba(0, 0, 0, 0.25) 0px 0.125em 0.5em,
    rgba(255, 255, 255, 0.1) 0px 0px 0px 1px inset;
  cursor: pointer;
  color: var(--gray-blue);
  transition: all 0.3s ease;
}

.download-table:hover {
  background: var(--gray-blue);
  color: var(--white);
  transform: scale(1.1);
  transition: transform 0.4s ease-in-out;
}

.add-word {
  border: none;
  width: 20%;
  font-family: "Montserrat-Bold", sans-serif;
  background: var(--main-gradient);
  font-size: 1.5rem;
  padding: 1rem 1rem;
  border-radius: 2rem;
  box-shadow:
    rgba(94, 94, 94, 0.25) 0px 0.0625em 0.0625em,
    rgba(0, 0, 0, 0.25) 0px 0.125em 0.5em,
    rgba(255, 255, 255, 0.1) 0px 0px 0px 1px inset;
  cursor: pointer;
  color: var(--white);
  transition: all 0.3s ease;
}

.add-word:hover {
  transform: scale(1.1);
  transition: transform 0.4s ease-in-out;
}

.filter {
  width: 40%;
  margin-bottom: 2rem;
  border: none;
  border-bottom: 1px solid var(--middle-dark-gray);
  font-family: "Montserrat-Regular", sans-serif;
  font-size: 1.5rem;
  padding: 10px 10px 10px 5px;
  color: var(--middle-dark-gray);
}

.bottom-section-items {
  display: grid;
  grid-template-columns: repeat(2, minmax(300px, 1fr));
  gap: 1rem;
  width: auto;
  max-width: 90%;
  margin: 0 auto;
}

.item-words {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  gap: 2rem;
}

.left-section {
  display: flex;
  align-items: center;
  gap: 2rem;
  font-size: 1.5rem;
  margin-right: 3rem;
}

.right-section {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}
</style>
