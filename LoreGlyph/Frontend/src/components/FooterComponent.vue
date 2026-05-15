<template>
  <div class="footer">
    <div class="left-footer">
      <a href="https://github.com/Springlezz/">Springlezz</a>
      <a href="https://github.com/Springlezz/LoreGlyph">GitHub Repository</a>
    </div>
    <div class="right-footer">
      <p>&copy; 2026 LoreGlyph</p>
    </div>
  </div>
</template>

<style scoped>
.footer {
  bottom: 0;
  left: 0;
  width: 100%;
  background: var(--black-gray);
  z-index: 1000;
  padding: 2rem;
  display: flex;
  color: var(--white);
  justify-content: space-between;
  margin-top: 2rem;
}

a {
  text-decoration: none;
  color: var(--white);
}

.right-footer {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  font-size: 1rem;
  color: var(--white);
  align-items: flex-end;
}

.left-footer {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  color: var(--white);
  font-size: 0.8rem;
  align-items: flex-start;
}

@media (min-width: 768px) {
  .left-footer {
    font-size: 1rem;
    padding: 0 6rem;
  }
}
</style>
