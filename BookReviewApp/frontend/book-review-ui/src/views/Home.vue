<script setup>
import { onMounted, ref } from "vue";
import api from "../api/api";

const books = ref([]);

const loadBooks = async () => {
    try {
        const response = await api.get("/books");

        books.value = response.data;
    }
    catch (err) {
        console.error(err);
    }
};

onMounted(() => {
    loadBooks();
});
</script>

<template>

    <div class="container">

        <h1>Book Review</h1>

<router-link to="/books/create">
    + Kitap Ekle
</router-link>
        <div
            class="book-card"
            v-for="book in books"
            :key="book.id"
        >

            <h2>{{ book.title }}</h2>

            <p>{{ book.author }}</p>

            <p>{{ book.publishedYear }}</p>

<router-link :to="`/books/${book.id}`">
    Detaya Git
</router-link>
        </div>

    </div>

</template>