<template>
  <div class="footer">
    <p>&copy; 2026 LoreGlyph</p>
    <a href="https://github.com/Springlezz/LoreGlyph">GitHub</a>
  </div>
</template>

<style scoped>
.footer {
  bottom: 0;
  left: 0;
  width: 100%;
  background: var(--dark-gray);
  z-index: 1000;
  padding: 1rem;
  display: flex;
  color: var(--white);
  justify-content: space-between;
  margin-top: 1rem;
  
}

@media (min-width: 768px){
  .footer {
    font-size: 1.5rem;
    padding: 4rem;
  }
}

a {
  text-decoration: none;
  color: var(--white);
}
</style>
