<template>
    <div>
        <button :disabled="count == 10" @click="increment" type="button" class="btn btn-primary">+1</button>
    </div>
</template>

<script>

export default {
    computed: {
        count() {
            return this.$store.getters.count
        }
    },
    methods: {
        increment() {
            this.$store.dispatch('increment')
        }
    }
}
</script>

<style lang="less">

</style>