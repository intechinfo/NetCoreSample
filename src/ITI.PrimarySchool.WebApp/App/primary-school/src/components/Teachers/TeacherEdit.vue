<template>
    <div>
        <div class="page-header">
            <h1 v-if="mode == 'create'">Créer un professeur</h1>
            <h1 v-else>Editer un professeur</h1>
        </div>

        <form @submit="onSubmit($event)">
            <div class="alert alert-danger" v-if="errors.length > 0">
                <b>Les champs suivants semblent invalides : </b>

                <ul>
                    <li v-for="e of errors">{{e}}</li>
                </ul>
            </div>

            <div class="form-group">
                <label class="required">Nom</label>
                <input type="text" v-model="item.lastName" class="form-control" required>
            </div>

            <div class="form-group">
                <label class="required">Prénom</label>
                <input type="text" v-model="item.firstName" class="form-control" required>
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
            ...mapGetters(['teacherList'])
        },

        created() {
            this.item = {};
            this.mode = this.$route.params.mode;
            this.id = this.$route.params.id;

            if(this.mode == 'edit') {
                let item = this.teacherList.find(x => x.teacherId == this.id);

                if(!item) this.$router.replace('/teachers');

                this.item = { ...item }
            }
        },

        methods: {
            ...mapActions(['createTeacher', 'updateTeacher']),

            onSubmit: async function(e) {
                e.preventDefault();

                var errors = [];

                if(!this.item.lastName) errors.push("Nom")
                if(!this.item.firstName) errors.push("Prénom")

                this.errors = errors;

                if(errors.length == 0) {
                    var result = null;

                    if(this.mode == 'create') {
                        result = await this.createTeacher(this.item);
                    }
                    else {
                        result = await this.updateTeacher(this.item);
                    }

                    if(result != null) this.$router.replace('/teachers');
                }
            }
        }
    }
</script>

<style lang="less">

</style>