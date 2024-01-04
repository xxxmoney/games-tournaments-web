// https://nuxt.com/docs/api/configuration/nuxt-config

export default defineNuxtConfig({
  devtools: { enabled: true },
  modules: [
      "@nuxtjs/tailwindcss",
      "nuxt-primevue"
  ],
  primevue: {
    unstyled: true,
    importPT: { from: '@/presets/wind/', as: 'wind' },
  },
  css: ['~/assets/css/main.css']
})