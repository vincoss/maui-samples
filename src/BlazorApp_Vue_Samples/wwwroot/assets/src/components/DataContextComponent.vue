<template>
    <div>
        <h1>DataContextComponent</h1>
        <button @click="onValidate">Validate</button>
        <button @click="onSave">Save</button>
        <textarea v-model="message" placeholder="add multiple lines" readonly></textarea>
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue, Watch } from "vue-property-decorator";
    import { DataContext } from '@/models/dataView';

    @Component({})
    export default class DataContextComponent extends Vue
    {
        @Prop() private model!: DataContext;

        private message = "";

        onValidate()
        {
            console.log("onValidate");
            this.model.validate(this.model.data);
        }

        onSave()
        {
            console.log("onSave");
            this.model.save(this.model.data);
        }

        @Watch("model", { immediate: true })
        @Watch("model.data.value", { immediate: true })
        async renderView()
        {
            console.log("renderView");

            if (this.model == null)
            {
                console.log("model is null!");
                return;
            }

            this.message = JSON.stringify(this.model);
        }
    }
</script>