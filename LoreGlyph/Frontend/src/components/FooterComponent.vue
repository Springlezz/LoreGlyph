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
  width: 100%;
  background: var(--black-gray);
  padding: 2rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  color: var(--white);
  margin-top: 2rem;
  box-sizing: border-box;
}

a {
  text-decoration: none;
  color: var(--white);
  transition: opacity 0.2s ease;
}

a:hover {
  opacity: 0.8;
}

.right-footer {
  font-size: 1rem;
  color: var(--white);
}

.left-footer {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  color: var(--white);
  font-size: 0.8rem;
}

@media (min-width: 768px) {
  .left-footer {
    font-size: 1rem;
  }

  .footer {
    padding: 2rem 4.5rem;
  }
}
</style>
