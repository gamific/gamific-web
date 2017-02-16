var teste = document.createElement("div");

var config = {
        container: "#custom-colored",

        nodeAlign: "BOTTOM",
        padding: 00,
        connectors: {
            type: 'step'
        },  
        node: {
            HTMLclass: 'tree'
        }
    },
    ceo = {
        text: {
            name: "Nome",
            title: "Diretor",
        },
        link: {
            href: "Dashboard.html"
        },
        //innerHTML: "<div class='sad'>HTML</div>",
        image: "img/profile-photo.jpg"
    },

    diretor1 = {
        parent: ceo,
        HTMLclass: 'chartNode',
        text:{
            name: "Nome",
            title: "Gerente",
        },
        link: {
            href: "Dashboard.html"
        },
        image: "plugins/treantJS/headshots/1.jpg"
    },
    diretor2 = {
        parent: ceo,
        childrenDropLevel: 2,
        HTMLclass: 'chartNode',
        text:{
            name: "Nome",
            title: "Gerente",
        },
        link: {
            href: "Dashboard.html"
        },
        image: "plugins/treantJS/headshots/5.jpg"
    },
    diretor3 = {
        parent: ceo,
        HTMLclass: 'chartNode',
        text:{
            name: "Nome",
            title: "Gerente",
        },
        link: {
            href: "Dashboard.html"
        },
        image: "plugins/treantJS/headshots/6.jpg"
    },
    sp1 = {
        parent: diretor1,
        HTMLclass: 'chartNode',
        text:{
            name: "Nome",
            title: "Supervisor",
        },
        link: {
            href: "Dashboard.html"
        },
        image: "plugins/treantJS/headshots/8.jpg"
    },
    sp2 = {
        parent: diretor1,
        HTMLclass: 'chartNode',
        text:{
            name: "Nome",
            title: "Supervisor",
        },
        link: {
            href: "Dashboard.html"
        },
        image: "plugins/treantJS/headshots/9.jpg"
    },
    sp3 = {
        parent: diretor2,
        HTMLclass: 'chartNode',
        text:{
            name: "Nome",
            title: "Supervisor",
        },
        link: {
            href: "Dashboard.html"
        },
        image: "plugins/treantJS/headshots/10.jpg"
    },
    sp4 = {
        parent: diretor2,
        HTMLclass: 'chartNode',
        text:{
            name: "Nome",
            title: "Supervisor",
        },
        link: {
            href: "Dashboard.html"
        },
        image: "plugins/treantJS/headshots/7.jpg"
    },
    sp5 = {
        parent: diretor3,
        HTMLclass: 'chartNode',
        text:{
            name: "Nome",
            title: "Supervisor",
        },
        link: {
            href: "Dashboard.html"
        },
        image: "plugins/treantJS/headshots/4.jpg"
    },
    sp6 = {
        parent: diretor3,
        HTMLclass: 'chartNode',
        text:{
            name: "Nome",
            title: "Supervisor",
        },
        link: {
            href: "Dashboard.html"
        },
        image: "plugins/treantJS/headshots/11.jpg"
    },

    chart_config = [
        config,
        ceo,diretor1,diretor2,diretor3,
        sp1,sp2,sp3,sp4,sp5,sp6
    ];

