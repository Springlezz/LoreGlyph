import { createApp } from "vue";
import { createRouter, createWebHistory } from "vue-router";

import App from "./App.vue";
import HomePage from "./views/HomePage.vue";
import "./views/AccountPage.vue";
import "./views/LanguagesPage.vue";
import "./views/WordsPage.vue";

import "./styles/reset.css";
import "./styles/colors.css";
import "./styles/fonts.css";
import "./styles/styles.css";

const routes = [
  { path: "/", redirect: "/home" },
  { path: "/home", name: "home", component: HomePage },
  {
    path: "/account",
    name: "account",
    component: () => import("./views/AccountPage.vue"),
  },
  {
    path: "/languages",
    name: "languages",
    component: () => import("./views/LanguagesPage.vue"),
  },
  {
    path: "/languages/words",
    name: "words",
    component: () => import("./views/WordsPage.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

createApp(App).use(router).mount("#app");
