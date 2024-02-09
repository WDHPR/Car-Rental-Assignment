const { addDynamicIconSelectors } = require("@iconify/tailwind");

/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./**/*.{razor,cshtml,html}"],
  theme: {
    extend: {},
  },
  plugins: [
    // Iconify plugin
    addDynamicIconSelectors(),
  ],
};
