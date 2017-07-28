import Vue from 'vue'
import $ from 'jquery'
import AuthService from '../services/AuthService'

export default Vue.directive('required-providers', {
    bind(el, binding) {
        var providers = binding.value;

        if(!providers || !providers instanceof Array) throw new Error("v-required-providers Expected Array value.");

        if(!AuthService.isBoundToProvider(providers)) {
            $(el).hide();
        }
    }
});

