<template>
    <div id="home">
        <div id="about" class="pl-12 pr-12 pt-8 pb-4">
            <h1 class="h0-mktg">BeatMods 2</h1>
            <p class="lead-mktg text-gray">
                A community maintained Beat Saber mod repository
            </p>
            <hr />
            <p>
                Need help installing mods? Check out the
                <a href="https://bsmg.wiki/beginners-guide">beginner's guide</a
                >.
            </p>
            <p>
                More questions?
                <a href="https://discord.gg/beatsabermods"
                    >Join the official Discord server.</a
                >
            </p>
            <hr />
            <p>
                Copyright (c) 2019 Anairkoen Schno & Raphaël Thériault &#8211;
                <a href="https://github.com/nike4613/BeatMods2"
                    >View on GitHub</a
                >
            </p>
        </div>
        <div id="news" class="pl-12 pr-12 pt-4 pb-8">
            <h2 class="h1">News</h2>
            <hr />
            <div class="Box mt-4">
                <div class="Box-header">
                    <div class="d-flex flex-justify-between flex-items-center">
                        <h3>Title</h3>
                        <span>Author &#8211; System</span>
                    </div>
                </div>
                <div class="Box-body markdown-body">
                    <p>
                        Spicy jalapeno bacon ipsum dolor amet andouille ground
                        round meatball, landjaeger alcatra ham tongue. T-bone
                        buffalo shank hamburger alcatra filet mignon. Shoulder
                        alcatra burgdoggen pork, tongue cow bresaola short loin
                        andouille drumstick ball tip. Frankfurter venison
                        picanha shoulder tail cupim short loin tri-tip pancetta
                        rump pork belly. Jerky shankle beef ribs pig pork chop.
                        Capicola biltong chicken porchetta ribeye pancetta.
                    </p>
                    <p>
                        Shoulder salami hamburger, bresaola turkey burgdoggen
                        flank t-bone chicken tail. Alcatra burgdoggen biltong
                        rump. Meatball pork loin kevin sirloin. Rump porchetta
                        burgdoggen chuck, jerky beef alcatra brisket tenderloin
                        kevin buffalo ham short loin turkey spare ribs. Chicken
                        meatball rump kielbasa. Filet mignon tri-tip venison
                        landjaeger bacon drumstick, salami tongue beef andouille
                        shankle chuck turducken. Ribeye ham pastrami porchetta
                        corned beef bacon hamburger turducken chicken cupim
                        tri-tip pork meatball fatback beef ribs.
                    </p>
                </div>
                <div class="Box-footer">
                    <div class="d-flex flex-justify-end flex-items-center">
                        <span
                            >Edited: 2019-09-27 22:54 &#8211; Published:
                            2019-09-27 22:53</span
                        >
                    </div>
                </div>
            </div>
            <div class="Box mt-4">
                <div class="Box-header">
                    <div class="d-flex flex-justify-between flex-items-center">
                        <h3>Title</h3>
                        <span>Author &#8211; System</span>
                    </div>
                </div>
                <div class="Box-body markdown-body">
                    <p>
                        Short loin prosciutto pastrami flank ground round. Rump
                        meatloaf andouille, jerky sirloin cow pastrami salami
                        short loin ham hock beef ribs boudin chuck. Hamburger
                        rump beef ribs filet mignon porchetta t-bone jowl
                        landjaeger turkey spare ribs. Doner bacon pancetta,
                        flank beef ribeye cow tenderloin corned beef pork chop
                        sausage shoulder strip steak capicola. Capicola pig
                        ribeye hamburger ball tip cupim.
                    </p>
                    <p>
                        Jerky brisket beef ribs, buffalo tail tri-tip jowl
                        drumstick rump pork belly short ribs shankle strip steak
                        cupim. Bacon jerky leberkas corned beef landjaeger
                        tri-tip burgdoggen alcatra fatback kielbasa chuck.
                        Buffalo flank chicken pancetta, tail ham swine leberkas
                        pork chop meatball shankle meatloaf. Burgdoggen jerky
                        buffalo bacon ham hock, pork belly porchetta tri-tip.
                        Ham hock sausage ham spare ribs.
                    </p>
                </div>
                <div class="Box-footer">
                    <div class="d-flex flex-justify-end flex-items-center">
                        <span>Published: 2019-09-27 23:02</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component({})
export default class Home extends Vue {}
</script>
