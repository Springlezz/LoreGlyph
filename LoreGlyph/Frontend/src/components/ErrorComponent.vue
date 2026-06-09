<template>
    <div class="modal">
        <h2>Ошибка</h2>
        <p>Ссылка удалена или не существует</p>
        
        <button @click="goToHome">На главную</button>
    </div>
</template>

<script setup>
import { useRouter } from "vue-router";

const router = useRouter();

const goToHome = () => {
    router.push("/home");
};
</script>