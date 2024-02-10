"""empty message

Revision ID: c7bc06c1ab53
Revises: 5d3bfe261503
Create Date: 2024-02-10 12:24:31.147110

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = 'c7bc06c1ab53'
down_revision = '5d3bfe261503'
branch_labels = None
depends_on = None


def upgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    op.create_table('stocks',
    sa.Column('id', sa.Integer(), nullable=False),
    sa.Column('ticker_symbol', sa.String(length=10), nullable=False),
    sa.Column('company_name', sa.String(length=100), nullable=False),
    sa.PrimaryKeyConstraint('id'),
    sa.UniqueConstraint('ticker_symbol')
    )
    # ### end Alembic commands ###


def downgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    op.drop_table('stocks')
    # ### end Alembic commands ###
