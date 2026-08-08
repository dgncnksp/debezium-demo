<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import api from "../api/api";

const router = useRouter();

const title = ref("");
const author = ref("");
const publishedYear = ref("");

const loading = ref(false);
const error = ref("");

const createBook = async () => {
    error.value = "";
    loading.value = true;

    try {
        const response = await api.post("/books", {
            title: title.value,
            author: author.value,
            publishedYear: Number(publishedYear.value)
        });

        await router.push(`/books/${response.data.id}`);
    } catch (err) {
        console.error(err);
        error.value = "Kitap eklenirken bir hata oluştu.";
    } finally {
        loading.value = false;
    }
};
</script>

<template>
    <div class="page">
        <div class="form-card">

```
        <div class="form-header">
            <h1>Yeni Kitap Ekle</h1>
            <p>Kitap bilgilerini girerek yeni bir kitap oluştur.</p>
        </div>

        <form @submit.prevent="createBook">

            <div class="form-group">
                <label for="title">Kitap Adı</label>

                <input
                    id="title"
                    v-model="title"
                    type="text"
                    placeholder="Örneğin: Suç ve Ceza"
                    required
                />
            </div>

            <div class="form-group">
                <label for="author">Yazar</label>

                <input
                    id="author"
                    v-model="author"
                    type="text"
                    placeholder="Örneğin: Fyodor Dostoyevski"
                    required
                />
            </div>

            <div class="form-group">
                <label for="publishedYear">Yayın Yılı</label>

                <input
                    id="publishedYear"
                    v-model="publishedYear"
                    type="number"
                    placeholder="Örneğin: 1866"
                    min="0"
                    required
                />
            </div>

            <p v-if="error" class="error">
                {{ error }}
            </p>

            <div class="actions">
                <router-link
                    to="/"
                    class="cancel-button"
                >
                    Vazgeç
                </router-link>

                <button
                    type="submit"
                    class="submit-button"
                    :disabled="loading"
                >
                    {{ loading ? "Ekleniyor..." : "Kitabı Ekle" }}
                </button>
            </div>

        </form>
    </div>
</div>
```

</template>

<style scoped>
.page {
    min-height: 100vh;
    display: flex;
    justify-content: center;
    align-items: flex-start;
    padding: 60px 20px;
    background: #f5f6f8;
}

.form-card {
    width: 100%;
    max-width: 560px;
    background: white;
    padding: 40px;
    border-radius: 16px;
    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.08);
}

.form-header {
    margin-bottom: 30px;
}

.form-header h1 {
    margin: 0 0 8px;
    font-size: 30px;
    color: #222;
}

.form-header p {
    margin: 0;
    color: #777;
}

.form-group {
    margin-bottom: 22px;
}

.form-group label {
    display: block;
    margin-bottom: 8px;
    font-size: 14px;
    font-weight: 600;
    color: #333;
}

.form-group input {
    width: 100%;
    box-sizing: border-box;
    padding: 13px 15px;
    border: 1px solid #dcdfe4;
    border-radius: 9px;
    font-size: 15px;
    outline: none;
    transition: border-color 0.2s, box-shadow 0.2s;
}

.form-group input:focus {
    border-color: #4f46e5;
    box-shadow: 0 0 0 3px rgba(79, 70, 229, 0.12);
}

.form-group input::placeholder {
    color: #aaa;
}

.actions {
    display: flex;
    justify-content: flex-end;
    gap: 12px;
    margin-top: 30px;
}

.cancel-button,
.submit-button {
    padding: 12px 20px;
    border-radius: 9px;
    font-size: 14px;
    font-weight: 600;
    cursor: pointer;
    text-decoration: none;
    transition: 0.2s;
}

.cancel-button {
    border: 1px solid #ddd;
    background: white;
    color: #555;
}

.cancel-button:hover {
    background: #f5f5f5;
}

.submit-button {
    border: none;
    background: #4f46e5;
    color: white;
}

.submit-button:hover {
    background: #4338ca;
}

.submit-button:disabled {
    opacity: 0.6;
    cursor: not-allowed;
}

.error {
    padding: 12px 14px;
    border-radius: 8px;
    background: #fff0f0;
    color: #d32f2f;
    font-size: 14px;
}
</style>
