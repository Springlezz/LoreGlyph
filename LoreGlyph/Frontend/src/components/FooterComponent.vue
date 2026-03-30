<template>
  <div class="footer">
    <p>&copy; 2026 LoreGlyph</p>
    <a href="https://github.com/Springlezz/LoreGlyph">GitHub</a>
  </div>
</template>

<style scoped>
.footer {
  flex-shrink: 0;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 4.3rem 7.5rem;
  width: 100%;
  height: 100px;
  background-color: var(--dark-gray);
  color: var(--white);
  font-size: 1.2rem;
}

a {
  text-decoration: none;
  color: var(--white);
}
</style>
