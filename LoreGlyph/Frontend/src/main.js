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
    path: "/profile",
    name: "profile",
    component: () => import("./views/AccountPage.vue"),
  },
  {
    path: "/languages",
    name: "languages",
    component: () => import("./views/LanguagesPage.vue"),
  },
  {
    path: "/language/:id",
    name: "words",
    component: () => import("./views/WordsPage.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

const isTokenValid = () => {
  const token = localStorage.getItem("token");
  if (!token) return false;

  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    const isValid = payload.exp * 1000 > Date.now();

    if (!isValid) {
      localStorage.removeItem("token");
      return false;
    }
    return true;
  } catch (error) {
    localStorage.removeItem("token");
    return false;
  }
};

router.beforeEach((to, from) => {
  const tokenValid = isTokenValid();
  const publicRoutes = ["/", "/home"];

  if (!publicRoutes.includes(to.path) && !tokenValid) {
    return "/home";
  }

  if (tokenValid && publicRoutes.includes(to.path)) {
    return "/languages";
  }

  return true;
});
createApp(App).use(router).mount("#app");
