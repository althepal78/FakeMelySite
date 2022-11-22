// const activePage = window.location.pathname;
// const navLinks = document.querySelectorAll('nav a')
// .forEach(link =>{
//     if(link.href.includes(`${activePage}`)){
//         link.classList.add('active');
//         link.classList.add('link')
//     }
// });

document.addEventListener("click", (e) => {
    const isDropdownButton = e.target.matches("[data-nav-dropdown-button]");

    if (!isDropdownButton && e.target.closest("[data-nav-dropdown]") != null)
        return;

    let currentDropdown;
    if (isDropdownButton) {
        currentDropdown = e.target.closest("[data-nav-dropdown]");
        // console.log(currentDropdown)
        currentDropdown.classList.toggle("active");
        console.log("is it clikcing");
    }

    document
        .querySelectorAll("[data-nav-dropdown].active")
        .forEach((dropdown) => {
            if (dropdown === currentDropdown) return;
            dropdown.classList.remove("active");
        });
});

document.querySelector("#cards").onmousemove = e => {
    for (const card of document.querySelectorAll(".card")) {
        const rect = card.getBoundingClientRect(),
            x = e.clientX - rect.left,
            y = e.clientY - rect.top;

        card.style.setProperty("--mouse-x", `${x}px`);
        card.style.setProperty("--mouse-y", `${y}px`);
    };
}