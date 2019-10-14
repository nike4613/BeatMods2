<template>
    <svg
        :height="height"
        :class="cssClasses"
        :viewBox="viewBox"
        version="1.1"
        aria-hidden="true"
        role="presentation"
        :width="width"
        :iconName="iconName"
        v-html="path"
    ></svg>
</template>

<script lang="ts">
import "./octicons";
import Octicons from "@primer/octicons";
import Vue from "vue";
import { Component, Prop } from "vue-property-decorator";

@Component({})
export default class Octicon extends Vue {
    iconHeight = 0;
    iconWidth = 0;
    @Prop({
        type: String,
        validator(name: string) {
            if (name) {
                if (Octicons[name]) {
                    return true;
                }
                return false;
            }
            return false;
        },
    })
    iconName: string | undefined;
    @Prop({
        type: String,
        default(): string {
            let self = this as Octicon;
            if (self.iconName) {
                let ico = Octicons[self.iconName];
                if (ico) {
                    return `0 0 ${ico.width} ${ico.height}`;
                }
            }
            return "0 0 12 16";
        },
    })
    viewBox: string | undefined;
    mounted() {
        if (this.$el) {
            this.iconHeight = parseFloat(
                window
                    .getComputedStyle(this.$el, null)
                    .getPropertyValue("font-size")
            );
            if (this.iconName) {
                let ico = Octicons[this.iconName];
                if (ico) {
                    this.iconWidth = (ico.width / ico.height) * this.iconHeight;
                }
            }
        }
    }
    // Add the default CSS class, the icon CSS class and any extra CSS classes together.
    get cssClasses() {
        return `octicon octicon-${this.iconName}`;
    }
    // Get the icon's path tag.
    get path() {
        if (this.iconName) {
            let ico = Octicons[this.iconName];
            if (ico) {
                return ico.path;
            }
        }
        return null;
    }
    // Get the icon height from the font-size of the element.
    get height() {
        return this.iconHeight || 0;
    }
    // Get the icon width from the icon size and the height of the element.
    get width() {
        return this.iconWidth || 0;
    }
}
</script>

<style lang="scss">
@import "@primer/octicons";
</style>
