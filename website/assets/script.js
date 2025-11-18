// Mobile menu toggle
(function () {
  var toggle = document.querySelector("[data-mobile-toggle]");
  var menu = document.querySelector("[data-mobile-menu]");
  if (!toggle || !menu) return;
  toggle.addEventListener("click", function () {
    menu.classList.toggle("open");
  });
})();
