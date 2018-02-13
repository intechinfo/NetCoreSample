import Vue from 'vue'

export default Vue.directive('bs-dropdown', {
    inserted(el, binding) {
        const container = el.parentNode;

        el.addEventListener('click', (e) => {
            container.classList.toggle('open');
        });
    }
}); 
 