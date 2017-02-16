var faqsSections = $('.cd-faq-group'),
faqTrigger = $('.cd-faq-trigger'),
faqsContainer = $('.cd-faq-items'),
faqsCategoriesContainer = $('.cd-faq-categories'),
faqsCategories = faqsCategoriesContainer.find('a'),
closeFaqsContainer = $('.cd-close-panel');

$('body').bind('click touchstart', function (event) {
    if ($(event.target).is('body.cd-overlay') || $(event.target).is('.cd-close-panel')) {
        closePanel(event);
    }
});
faqsContainer.on('swiperight', function (event) {
    closePanel(event);
});

faqTrigger.on('click', function (event) {
    event.preventDefault();
    $(this).next('.cd-faq-content').slideToggle(200).end().parent('li').toggleClass('content-visible');
});