<template>
    <div>
        <div class="page-header">
            <h1 v-if="mode == 'create'">Créer une classe</h1>
            <h1 v-else>Editer une classe</h1>
        </div>

        <form @submit="onSubmit($event)">
            <div class="alert alert-danger" v-if="errors.length > 0">
                <b>Les champs suivants semblent invalides : </b>

                <ul>
                    <li v-for="e of errors">{{e}}</li>
                </ul>
            </div>

            <div class="form-group">
                <label class="required">Nom de la classe</label>
                <input type="text" v-model="item.name" class="form-control" required>
            </div>

            <div class="form-group">
                <label class="required">Niveau</label>
                <select v-model="item.level" class="form-control" required>
                    <option>CP</option>
                    <option>CE1</option>
                    <option>CE2</option>
                    <option>CM1</option>
                    <option>CM2</option>
                </select>
            </div>

            <button type="submit" class="btn btn-primary">Sauvegarder</button>
        </form>
    </div>
</template>

<script>
    import { mapGetters, mapActions } from 'vuex'

    export default {
        data () {
            return {
                item: {},
                mode: null,
                id: null,
                errors: []
            }
        },

        computed: {
            ...mapGetters(['classList'])
        },

        created() {
            this.item = {};
            this.mode = this.$route.params.mode;
            this.id = this.$route.params.id;

            if(this.mode == 'edit') {
                let item = this.classList.find(x => x.classId == this.id);

                if(!item) this.$router.replace('/classes');

                this.item = { ...item }
            }
        },

        methods: {
            ...mapActions(['createClass', 'updateClass']),

            onSubmit: async function(e) {
                e.preventDefault();

                // Google Chrome handles form validation based on type of the input, and presence of the "required" attribute.
                // However, it's not (yet) fully supported by all the web browsers.
                // Therefore, the code below handles validation but is very naive: a better validation is desirable.
                var errors = [];

                if(!this.item.name) errors.push("Nom de la classe")
                if(!this.item.level) errors.push("Niveau")

                this.errors = errors;

                if(errors.length == 0) {
                    var result = null;

                    if(this.mode == 'create') {
                        result = await this.createClass(this.item);
                    }
                    else {
                        result = await this.updateClass(this.item);
                    }

                    if(result != null) this.$router.replace('/classes');
                }
            }
        }
    }
</script>

<style lang="less">

</style>